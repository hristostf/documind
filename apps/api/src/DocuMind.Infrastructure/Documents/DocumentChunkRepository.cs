
using DocuMind.Domain.Documents;
using DocuMind.Application.Documents;
using DocuMind.Infrastructure.Persistence;

namespace DocuMind.Infrastructure.Documents;

internal sealed class DocumentChunkRepository (
    DocuMindDbContext dbContext) : IDocumentChunkRepository
{
    public async Task AddRangeAsync(
        IReadOnlyCollection<DocumentChunk> documentChunks,
        CancellationToken cancellationToken = default)
    {
        await dbContext.DocumentChunks.AddRangeAsync(documentChunks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}