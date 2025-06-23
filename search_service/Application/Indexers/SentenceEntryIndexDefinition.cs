using Application.Domains.DTOs;
using Application.Interfaces;

namespace Application.Indexers;

using Nest;

public class SentenceEntryIndexDefinition : IElasticIndexDefinition<SentenceEntry>
{
    public TypeMappingDescriptor<SentenceEntry> BuildMapping(TypeMappingDescriptor<SentenceEntry> map)
    {
        return map.Properties(p => p
            .Keyword(k => k.Name(n => n.DocumentId))
            .Number(nu => nu.Name(n => n.Page).Type(NumberType.Integer))
            .Keyword(k => k.Name(n => n.CaseId))
            .Keyword(k => k.Name(n => n.AttachmentId))
            .Text(t => t.Name(n => n.Sentence))
            .Nested<PdfWord>(n => n
                .Name(nn => nn.Words)
                .Properties(np => np
                    .Keyword(k => k.Name(w => w.Word))
                    .Object<Position>(o => o
                        .Name(po => po.Position)
                        .Properties(pp => pp
                            .Number(nn => nn.Name(p => p.X1).Type(NumberType.Float))
                            .Number(nn => nn.Name(p => p.X2).Type(NumberType.Float))
                            .Number(nn => nn.Name(p => p.Y1).Type(NumberType.Float))
                            .Number(nn => nn.Name(p => p.Y2).Type(NumberType.Float))
                        )
                    )
                )
            )
        );
    }
}
