using Application.Domains.DTOs;
using UglyToad.PdfPig;


namespace Application.Parsers;

public static class CaseWithPosPdfParser
{
    /// <summary>
    /// Parser en PDF fra en byte-array og konverterer hver linje på hver side til en <see cref="SentenceEntry"/>, 
    /// som indeholder sætningsteksten og positionsdata for hvert ord.
    /// </summary>
    /// <param name="pdfBytes">PDF-filen som byte-array.</param>
    /// <param name="documentId">Et unikt ID for dokumentet.</param>
    /// <param name="caseId">(Optional) ID for tilknyttet sag.</param>
    /// <param name="attachmentId">(Optional) ID for vedhæftning.</param>
    /// <param name="fileName">(Optional) Navn på den oprindelige fil.</param>
    /// <returns>En liste af <see cref="SentenceEntry"/> objekter, én for hver tekstlinje i PDF'en.</returns>
    public static List<SentenceEntry> Parse(
        byte[] pdfBytes,
        string documentId,
        string? caseId = null,
        string? attachmentId = null,
        string? fileName = null)
    {
        using var stream = new MemoryStream(pdfBytes);
        using var document = PdfDocument.Open(stream);

        return (from page in document.GetPages()
            let lines = page.GetWords()
                .GroupBy(w => Math.Round(w.BoundingBox.Top, 1))
                .OrderBy(g => g.Key)
            from lineGroup in lines
            let sortedWords = lineGroup.OrderBy(w => w.BoundingBox.Left).ToList()
            let sentenceText = string.Join(" ", sortedWords.Select(w => w.Text))
            let wordObjects = sortedWords.Select(w => new PdfWord { Word = w.Text, Position = new Position { X1 = (float)w.BoundingBox.Left, X2 = (float)w.BoundingBox.Right, Y1 = (float)w.BoundingBox.Top, Y2 = (float)w.BoundingBox.Bottom } }).ToList()
            select new SentenceEntry
            {
                DocumentId = documentId,
                Page = page.Number,
                Sentence = sentenceText,
                Words = wordObjects,
                CaseId = caseId,
                AttachmentId = attachmentId,
                FileName = fileName
            }).ToList();
    }
}



