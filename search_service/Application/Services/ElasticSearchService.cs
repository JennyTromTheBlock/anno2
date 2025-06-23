using Application.Domains.DTOs;
using Application.Domains.Enums;
using Application.Indexers;
using Application.Interfaces.Services;
using Application.Parsers;
using Nest;

namespace Application.Services;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticClient _client;
    private readonly ElasticIndexManager _elasticManager;
    
    private const int IndexingBatchSize = 5000; // todo skal være i config eller noget 


    public ElasticSearchService(IElasticClient client, ElasticIndexManager elasticManager)
    {

        _client = client;
        _elasticManager = elasticManager;
    }


    public async Task IndexPdfAsync(
        byte[] pdfBytes,
        string documentId,
        string? caseId = null,
        string? attachmentId = null,
        string? fileName = null)
    {
        var sentenceEntries = CaseWithPosPdfParser.Parse(pdfBytes, documentId, caseId, attachmentId, fileName);

        if (sentenceEntries.Count == 0)
        {
            throw new InvalidOperationException("Ingen ord/sætninger fundet i PDF-dokumentet.");
        }

        for (var i = 0; i < sentenceEntries.Count; i += IndexingBatchSize)
        {
            var batch = sentenceEntries.Skip(i).Take(IndexingBatchSize);

            var bulkResponse = await _client.BulkAsync(b => b
                .Index(ElasticIndex.PdfWords.ToString().ToLower())
                .IndexMany(batch)
            );

            if (!bulkResponse.Errors) continue;
            var errors = string.Join("\n", bulkResponse.ItemsWithErrors.Select(e => $"Id: {e.Id}, Error: {e.Error.Reason}"));
            throw new Exception("Fejl under bulk-indeksering: \n" + errors);
        }
    }


    public async Task<IEnumerable<PdfWord>> SearchWordPositionsAsync(CaseSearchQueryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Query))
            throw new ArgumentException("searchTerm må ikke være tom", nameof(dto.Query));
    
        var response = await _client.SearchAsync<SentenceEntry>(s => s
                .Index(ElasticIndex.PdfWords.ToString().ToLower())
                .Size(1000) // antal hits der maks returneres
                .Query(q => q
                    .Nested(n => n
                        .Path(p => p.Words)
                        .Query(nq => nq
                            .Match(m => m
                                .Field(f => f.Words.First().Word) // match på ordet i Words-listen
                                .Query(dto.Query)
                                .Operator(Operator.And)
                            )
                        )
                    )
                )
                .Source(sf => sf.Includes(i => i.Field(f => f.Words))) // kun words feltet returneres
        );

        if (!response.IsValid)
            throw new Exception("Elasticsearch query failed: " + response.DebugInformation);

        var matchedWords = new List<PdfWord>();

        foreach (var sentence in response.Documents)
        {
            // filtrer kun de ord der matcher søgningen
            matchedWords.AddRange(
                sentence.Words.Where(w => 
                    w.Word != null && 
                    w.Word.Equals(dto.Query, StringComparison.OrdinalIgnoreCase))
            );
        }

        return matchedWords;
    }
    
    
public async Task<IEnumerable<PdfWithWordsReturnDto>> GetPdfWithWordsAsync(CaseSearchQueryDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Query))
        throw new ArgumentException("Query må ikke være tom.", nameof(dto.Query));

    var searchTerms = dto.Query
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .ToList();

    var useSlop = dto.Slop.HasValue && searchTerms.Count > 1;

    var response = await _client.SearchAsync<SentenceEntry>(s => s
        .Index(ElasticIndex.PdfWords.ToString().ToLower())
        .Size(dto.batchSize > 0 ? dto.batchSize : 1000)
        .Query(q => q
            .Bool(b => b
                .Must(useSlop
                    ? new Func<QueryContainerDescriptor<SentenceEntry>, QueryContainer>[] {
                        q => q.MatchPhrase(mp => mp
                            .Field(f => f.Sentence)
                            .Query(dto.Query)
                            .Slop(dto.Slop)
                        )
                    }
                    : searchTerms.Select(term =>
                        (Func<QueryContainerDescriptor<SentenceEntry>, QueryContainer>)(q =>
                            q.Nested(n => n
                                .Path(p => p.Words)
                                .Query(nq => nq
                                    .Match(m => m
                                        .Field(f => f.Words.First().Word)
                                        .Query(term)
                                        .Fuzziness(dto.Fuzzy == true ? Fuzziness.Auto : null)
                                    )
                                )
                            )
                        )
                    ).ToArray()
                )
                .Filter(f =>
                {
                    QueryContainer container = null;

                    if (!string.IsNullOrWhiteSpace(dto.DocumentId))
                        container &= f.Term("documentId", dto.DocumentId);
                    if (!string.IsNullOrWhiteSpace(dto.CaseId))
                        container &= f.Term("caseId", dto.CaseId);
                    if (!string.IsNullOrWhiteSpace(dto.AttachmentId))
                        container &= f.Term("attachmentId", dto.AttachmentId);

                    return container;
                })
            )
        )
        .Source(sf => sf
            .Includes(i => i
                .Fields(
                    f => f.DocumentId,
                    f => f.Sentence,
                    f => f.Page,
                    f => f.Words
                )
            )
        )
        .Highlight(h => h
            .Fields(hf => hf
                .Field(useSlop ? "sentence" : "words.word")
                .PreTags("<hit>")
                .PostTags("</hit>")
            )
        )
    );

    if (!response.IsValid)
        throw new Exception("Elasticsearch query failed: " + response.DebugInformation);

    var results = new List<PdfWithWordsReturnDto>();

    foreach (var hit in response.Hits)
    {
        var sentence = hit.Source;
        var matchedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (useSlop && hit.Highlight.TryGetValue("sentence", out var sentenceHighlights))
        {
            foreach (var fragment in sentenceHighlights)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(fragment, "<hit>(.*?)</hit>");
                foreach (System.Text.RegularExpressions.Match match in matches)
                    matchedWords.UnionWith(match.Groups[1].Value.Split(' ')); // evt. flere ord
            }
        }
        else if (hit.Highlight.TryGetValue("words.word", out var wordHighlights))
        {
            foreach (var fragment in wordHighlights)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(fragment, "<hit>(.*?)</hit>");
                foreach (System.Text.RegularExpressions.Match match in matches)
                    matchedWords.Add(match.Groups[1].Value);
            }
        }

        var filteredWords = sentence.Words
            .Where(w => matchedWords.Contains(w.Word, StringComparer.OrdinalIgnoreCase))
            .ToList();

        results.Add(new PdfWithWordsReturnDto
        {
            DocumentId = sentence.DocumentId,
            Sentence = sentence.Sentence,
            Page = sentence.Page,
            Words = filteredWords
        });
    }

    return results;
}



    
    //re inits all indexes i elastic container
    //skal ikke kunne gøres i live miljø uden admin tilgang
    public async Task InitElasticSearch()
    {
        await _elasticManager.DeleteAllIndicesAsync();
        await _elasticManager.CreateAllIndicesAsync();
    }
}

