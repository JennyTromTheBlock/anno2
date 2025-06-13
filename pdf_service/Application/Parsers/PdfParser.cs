namespace Application.Parsers;

using UglyToad.PdfPig;

public static class PdfParser
{
    public static List<PdfWordEntry> Parse(byte[] pdfBytes, string documentId)
    {
        var entries = new List<PdfWordEntry>();

        using var stream = new MemoryStream(pdfBytes);
        using var document = PdfDocument.Open(stream);

        foreach (var page in document.GetPages())
        {
            var pageNumber = page.Number;

            foreach (var word in page.GetWords())
            {
                var bounds = word.BoundingBox;
                entries.Add(new PdfWordEntry
                {
                    DocumentId = documentId,
                    Page = pageNumber,
                    Word = word.Text,
                    Position = new Position
                    {
                        X = (float)bounds.Left,
                        Y = (float)bounds.Top,
                        Width = (float)bounds.Width,
                        Height = (float)bounds.Height
                    }
                });
            }
        }

        return entries;
    }
}
