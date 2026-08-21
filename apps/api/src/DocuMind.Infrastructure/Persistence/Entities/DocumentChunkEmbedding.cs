using Pgvector;

namespace DocuMind.Infrastructure.Persistence.Entities;

internal sealed class DocumentChunkEmbedding
{
    public Guid DocumentChunkId { get; set; }

    public Vector Embedding { get; set; } = null!;
}