using Application.Domains.DTOs;
using UglyToad.PdfPig;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Application.Parsers
{
    public static class CaseWithPosPdfParser
    {
        public static List<SentenceEntry> Parse(
            byte[] pdfBytes,
            string documentId,
            string? caseId = null,
            string? attachmentId = null,
            string? fileName = null)
        {
            var entries = new List<SentenceEntry>();

            using var stream = new MemoryStream(pdfBytes);
            using var document = PdfDocument.Open(stream);

            foreach (var page in document.GetPages())
            {
                // Grupper ord på samme linje baseret på Top (med lidt tolerance)
                var lines = page.GetWords()
                    .GroupBy(w => Math.Round(w.BoundingBox.Top, 1))
                    .OrderBy(g => g.Key);

                foreach (var lineGroup in lines)
                {
                    var sortedWords = lineGroup.OrderBy(w => w.BoundingBox.Left).ToList();

                    var sentenceText = string.Join(" ", sortedWords.Select(w => w.Text));

                    // Lav PdfWord-listen med ord og positioner
                    var wordObjects = sortedWords.Select(w => new PdfWord
                    {
                        Word = w.Text,
                        Position = new Position
                        {
                            X1 = (float)w.BoundingBox.Left,
                            X2 = (float)w.BoundingBox.Right,
                            Y1 = (float)w.BoundingBox.Top,
                            Y2 = (float)w.BoundingBox.Bottom
                        }
                    }).ToList();

                    // Lav SentenceEntry med nested words
                    entries.Add(new SentenceEntry
                    {
                        DocumentId = documentId,
                        Page = page.Number,
                        Sentence = sentenceText,
                        Words = wordObjects,
                        CaseId = caseId,
                        AttachmentId = attachmentId,
                        FileName = fileName
                    });
                }
            }

            return entries;
        }
    }
}


