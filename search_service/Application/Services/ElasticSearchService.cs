using Application.Domains.DTOs;
using Application.Interfaces.Services;
using Application.Parsers;
using Nest;

public class ElasticSearchService : IElasticSearchService
{
    private readonly IElasticClient _client;

    public ElasticSearchService(string elasticUrl)
    {
        var settings = new ConnectionSettings(new Uri(elasticUrl))
            .DefaultIndex("pdfwords");
        _client = new ElasticClient(settings);
    }

    private const int BatchSize = 5000; // todo skal være i config eller noget 

    public async Task IndexPdfAsync(byte[] pdfBytes, string documentId)
    {
        var wordEntries = CaseWithPosPdfParser.Parse(pdfBytes, documentId);

        if (wordEntries.Count == 0)
        {
            throw new InvalidOperationException("Ingen ord fundet i PDF-dokumentet.");
        }


        for (int i = 0; i < wordEntries.Count; i += BatchSize)
        {
            var batch = wordEntries.Skip(i).Take(BatchSize);

            var response = await _client.BulkAsync(b => b
                .Index("pdfwords")
                .IndexMany(batch)
            );

            if (response.Errors)
            { 
                throw new Exception("Fejl under bulk-indeksering: " + response.ServerError?.ToString());
            }
        }
    }



    public async Task<IEnumerable<PdfWordEntry>> SearchAsync(string word, string? documentId = null)
    {
        var response = await _client.SearchAsync<PdfWordEntry>(s => s
            .Index("pdfwords")
            .Query(q =>
            {
                QueryContainer query = q.Match(m => m.Field(f => f.Word).Query(word));
                if (!string.IsNullOrEmpty(documentId))
                {
                    query &= q.Term(t => t.Field(f => f.DocumentId).Value(documentId));
                }
                return query;
            })
            .Size(1000)
        );
        return response.Documents;
    }
}
