

using DocuMind.Domain.Documents;
using DocuMind.Application.Documents;
using Microsoft.EntityFrameworkCore;
using DocuMind.Infrastructure.Persistence;


namespace DocuMind.Infrastructure.Documents;


internal sealed class DocumentRepository(
    DocuMindDbContext dbContext)
    : IDocumentRepository
{
    public async Task AddAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Documents.AddAsync(document, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Document?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Documents
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Document>> ListByWorkspaceIdAsync(
        Guid workspaceId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Documents
            .AsNoTracking()
            .Where(x => x.WorkspaceId == workspaceId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        dbContext.Documents.Update(document);

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }

}