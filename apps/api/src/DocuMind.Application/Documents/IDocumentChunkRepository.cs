namespace DocuMind.Application.Documents;

using DocuMind.Domain.Documents;
public interface IDocumentChunkRepository
{
    Task AddRangeAsync(
        IReadOnlyCollection<DocumentChunk> documentChunks,
        CancellationToken cancellationToken = default);
}