namespace DefaultNamespace;

using Application.Parsers;
using Application.Interfaces;
using Nest;

namespace Application.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _client;

        public ElasticSearchService(string elasticUrl)
        {
            var settings = new ConnectionSettings(new Uri(elasticUrl))
                .DefaultIndex("pdfwords");
            _client = new ElasticClient(settings);
        }

        public async Task IndexPdfAsync(byte[] pdfBytes, string documentId)
        {
            var wordEntries = PdfParser.Parse(pdfBytes, documentId);

            var response = await _client.BulkAsync(b => b
                .Index("pdfwords")
                .IndexMany(wordEntries)
            );

            if (response.Errors)
            {
                throw new Exception("Fejl under bulk-indeksering: " + response.ServerError?.ToString());
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
}
