using DocuMind.Application.Documents.Processing;
using DocuMind.Application.Documents.Search;
using DocuMind.Application.Workspaces;

namespace DocuMind.Application.Documents.Ask;

public sealed class AskDocumentsHandler(
    IWorkspaceRepository workspaceRepository,
    IEmbeddingGenerator embeddingGenerator,
    IDocumentSearchRepository documentSearchRepository,
    IAnswerGenerator answerGenerator)
{
    public async Task<AskDocumentsResult?> HandleAsync(
        AskDocumentsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Question))
        {
            return new AskDocumentsResult(
            "Question is required.");
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
            query.Question.Trim(),
            cancellationToken);

        var searchResults = await documentSearchRepository.SearchAsync(
            query.WorkspaceId,
            embedding,
            limit,
            cancellationToken);

        if (searchResults.Count == 0)
        {
            return new AskDocumentsResult(
                "I could not find relevant information in the workspace documents.");
        }

        var contextChunks = searchResults
            .Select(x => x.Content)
            .ToList();

        var answer = await answerGenerator.GenerateAsync(
            query.Question.Trim(),
            contextChunks,
            cancellationToken);

        return new AskDocumentsResult(answer);
    }
}