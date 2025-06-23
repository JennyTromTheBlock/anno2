using Application.Domains.DTOs;
using Application.Domains.Enums;
using Application.Interfaces;
using Nest;

namespace Application.Indexers;

public class ElasticIndexManager
{
    private readonly IElasticClient _client;

    public ElasticIndexManager(IElasticClient client)
    {
        _client = client;
    }

    private async Task CreateIndexAsync<T>(string indexName, IElasticIndexDefinition<T> definition) where T : class
    {
        var exists = await _client.Indices.ExistsAsync(indexName);
        if (exists.Exists)
        {
            await _client.Indices.DeleteAsync(indexName);
        }

        var response = await _client.Indices.CreateAsync(indexName, c => c
            .Map<T>(definition.BuildMapping)
        );

        if (!response.IsValid)
        {
            throw new Exception($"Fejl under oprettelse af index {indexName}: {response.ServerError?.Error.Reason}");
        }
    }

    public async Task CreateAllIndicesAsync()
    {
        foreach (var kv in ElasticIndexDefinitions.Definitions)
        {
            var indexName = kv.Key.ToString().ToLowerInvariant();

            switch (kv.Value)
            {
                case IElasticIndexDefinition<SentenceEntry> sentenceEntryDef:
                    await CreateIndexAsync(indexName, sentenceEntryDef);
                    break;

                // Tilføj flere case-blocks her hvis du har flere typer
                default:
                    throw new InvalidOperationException($"Ukendt definitionstype for index {kv.Key}");
            }
        }
    }
    
    public async Task DeleteAllIndicesAsync()
    {
        foreach (var indexName in ElasticIndexDefinitions.Definitions.Select(kv => kv.Key.ToString().ToLowerInvariant()))
        {
            var exists = await _client.Indices.ExistsAsync(indexName);
            if (!exists.Exists) continue;
            
            var deleteResponse = await _client.Indices.DeleteAsync(indexName);
            if (!deleteResponse.IsValid)
            {
                throw new Exception($"Fejl ved sletning af index {indexName}: {deleteResponse.ServerError?.Error.Reason}");
            }
        }
    }
}
