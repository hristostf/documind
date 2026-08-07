namespace DocuMind.Application.Documents.Processing;

public interface IDocumentTextExtractor
{
    Task<string> ExtractTextAsync(
        string storageKey,
        CancellationToken cancellationToken = default);
}