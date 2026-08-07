namespace DocuMind.Application.Documents.Processing;
    
public interface IDocumentProcessingQueue
{
    ValueTask EnqueueAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    ValueTask<Guid> DequeueAsync(
        CancellationToken cancellationToken);
}       