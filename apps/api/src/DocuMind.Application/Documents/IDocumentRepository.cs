

using DocuMind.Domain.Documents;

namespace DocuMind.Application.Documents;

public interface IDocumentRepository
{
    Task AddAsync(
        Document document,
        CancellationToken cancellationToken = default);

    Task<Document?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Document>> ListByWorkspaceIdAsync(
        Guid workspaceId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Document document,
        CancellationToken cancellationToken = default);

}