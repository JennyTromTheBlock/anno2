
using Application.Indexers;

namespace Application.Domains.Enums;

public enum ElasticIndex
{
    PdfWords
}


public static class ElasticIndexDefinitions
{
    public static readonly Dictionary<ElasticIndex, object> Definitions = new()
    {
        { ElasticIndex.PdfWords, new SentenceEntryIndexDefinition() }
        // Tilføj flere her når du har nye definitioner
    };
}

