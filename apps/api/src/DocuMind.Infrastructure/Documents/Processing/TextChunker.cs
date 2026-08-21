using DocuMind.Application.Documents.Processing;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class TextChunker : ITextChunker
{
    private const int ChunkSize = 1500;
    private const int Overlap = 200;

    public IReadOnlyList<string> Chunk(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var chunks = new List<string>();

        var start = 0;

        while (start < text.Length)
        {
            var length = Math.Min(
                ChunkSize,
                text.Length - start);

            var chunk = text
                .Substring(start, length)
                .Trim();

            if (!string.IsNullOrWhiteSpace(chunk))
            {
                chunks.Add(chunk);
            }

            if (start + length >= text.Length)
            {
                break;
            }

            start += ChunkSize - Overlap;
        }

        return chunks;
    }
}