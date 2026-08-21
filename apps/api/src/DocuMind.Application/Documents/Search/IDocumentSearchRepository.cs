namespace DocuMind.Application.Documents.Search;

public interface IDocumentSearchRepository
{
    Task<IReadOnlyList<DocumentSearchResult>> SearchAsync(
        Guid workspaceId,
        IReadOnlyList<float> queryEmbedding,
        int limit,
        CancellationToken cancellationToken = default);
}