using Application.Domains.DTOs;
using Application.Interfaces.Services;
using Application.Parsers;
using DefaultNamespace;
using Nest;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticClient _client;
    private readonly ElasticIndexManager _elasticManager;
    
    private const int IndexingBatchSize = 5000; // todo skal være i config eller noget 


    public ElasticSearchService(string elasticUrl, ElasticIndexManager elasticManager)
    {
        var settings = new ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("pdfwords");
        
        _client = new ElasticClient(settings);
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
                .Index("pdfwords")
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
                .Index("pdfwords")
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
    
    //re inits all indexes i elastic container
    //skal ikke kunne gøres i live miljø uden admin tilgang
    public async Task InitElasticSearch()
    {
        await _elasticManager.DeleteAllIndicesAsync();
        await _elasticManager.CreateAllIndicesAsync();
    }
}
