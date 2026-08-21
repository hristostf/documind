namespace DocuMind.Application.Documents.Processing;

public interface ITextChunker
{
    IReadOnlyList<string> Chunk(
        string text);
}