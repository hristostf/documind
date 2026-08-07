

namespace DocuMind.Application.Documents.Processing;

public interface IDocumentProcessor
{
    Task ProcessAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);
}