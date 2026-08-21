namespace DocuMind.Application.Documents.Search;

using DocuMind.Application.Documents.Processing;
using DocuMind.Application.Workspaces;

public sealed class SearchDocumentsHandler(
    IWorkspaceRepository workspaceRepository,
    IEmbeddingGenerator embeddingGenerator,
    IDocumentSearchRepository documentSearchRepository)
{
    public async Task<IReadOnlyList<DocumentSearchResult>?> HandleAsync(
        SearchDocumentsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
        {
            return [];
        }

        var workspace = await workspaceRepository.GetByIdAsync(
            query.WorkspaceId,
            cancellationToken);

        if (workspace is null ||
            workspace.OrganizationId != query.OrganizationId)
        {
            return null;
        }

        var limit = Math.Clamp(
            query.Limit,
            1,
            20);

        var embedding = await embeddingGenerator.GenerateAsync(
            query.Query.Trim(),
            cancellationToken);

        return await documentSearchRepository.SearchAsync(
            query.WorkspaceId,
            embedding,
            limit,
            cancellationToken);
    }
}