namespace DocuMind.Application.Documents;

public interface IDocumentChunkEmbeddingRepository
{
    Task AddAsync(
        Guid documentChunkId,
        IReadOnlyList<float> embedding,
        CancellationToken cancellationToken = default);
}