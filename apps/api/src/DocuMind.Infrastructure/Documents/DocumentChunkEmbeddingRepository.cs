using Pgvector;
using DocuMind.Application.Documents;
using DocuMind.Infrastructure.Persistence;
using DocuMind.Infrastructure.Persistence.Entities;

namespace DocuMind.Infrastructure.Documents;

internal sealed class DocumentChunkEmbeddingRepository(
    DocuMindDbContext dbContext) : IDocumentChunkEmbeddingRepository
{
    public async Task AddAsync(
        Guid documentChunkId,
        IReadOnlyList<float> embedding,
        CancellationToken cancellationToken = default)
    {
        var documentChunkEmbedding = new DocumentChunkEmbedding
        {
            DocumentChunkId = documentChunkId,
            Embedding = new Vector(embedding.ToArray())
        };

        await dbContext.DocumentChunkEmbeddings.AddAsync(documentChunkEmbedding, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}