using System.Threading.Channels;
using DocuMind.Application.Documents.Processing;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class DocumentProcessingQueue
    : IDocumentProcessingQueue
{
    private readonly Channel<Guid> _channel =
        Channel.CreateUnbounded<Guid>();

    public ValueTask EnqueueAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(
            documentId,
            cancellationToken);
    }

    public ValueTask<Guid> DequeueAsync(
        CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAsync(
            cancellationToken);
    }
}