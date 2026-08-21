using DocuMind.Application.Documents.Search;
using DocuMind.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace DocuMind.Infrastructure.Documents;

internal sealed class DocumentSearchRepository(
    DocuMindDbContext dbContext)
    : IDocumentSearchRepository
{
    public async Task<IReadOnlyList<DocumentSearchResult>> SearchAsync(
        Guid workspaceId,
        IReadOnlyList<float> queryEmbedding,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var vector = new Vector(
            queryEmbedding.ToArray());

        return await (
            from embedding in dbContext.DocumentChunkEmbeddings
            join chunk in dbContext.DocumentChunks
                on embedding.DocumentChunkId equals chunk.Id
            join document in dbContext.Documents
                on chunk.DocumentId equals document.Id
            where document.WorkspaceId == workspaceId
            orderby embedding.Embedding.CosineDistance(vector)
            select new DocumentSearchResult(
                chunk.Id,
                document.Id,
                chunk.Content,
                embedding.Embedding.CosineDistance(vector)))
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}