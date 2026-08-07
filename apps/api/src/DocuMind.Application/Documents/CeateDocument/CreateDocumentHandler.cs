

using DocuMind.Application.Workspaces;
using DocuMind.Domain.Documents;

namespace DocuMind.Application.Documents.CreateDocument;


public sealed class CreateDocumentHandler(
    IWorkspaceRepository workspaceRepository,
    IDocumentRepository documentRepository)
{
  
    public async Task<CreateDocumentResponse> HandleAsync(
        CreateDocumentCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = command.Name?.Trim();
        var normalizedOriginalFileName = command.OriginalFileName?.Trim();
        var normalizedContentType = command.ContentType?.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            return CreateDocumentResponse.Failure(
                CreateDocumentError.NameRequired);
        }

        if (string.IsNullOrWhiteSpace(normalizedOriginalFileName))
        {
            return CreateDocumentResponse.Failure(
                CreateDocumentError.OriginalNameRequired);
        }

        if (string.IsNullOrWhiteSpace(normalizedContentType))
        {
            return CreateDocumentResponse.Failure(
                CreateDocumentError.ContentTypeRequired);
        }

        if (command.SizeInBytes <= 0)
        {
            return CreateDocumentResponse.Failure(
                CreateDocumentError.InvalidSizeInBytes);
        }

        var workspace = await workspaceRepository.GetByIdAsync(
        command.WorkspaceId,
        cancellationToken);

        if (workspace is null)
        {
            return CreateDocumentResponse.Failure(
                CreateDocumentError.WorkspaceNotFound);
        }

        if (workspace.OrganizationId != command.OrganizationId)
        {
                return CreateDocumentResponse.Failure(
                    CreateDocumentError.WorkspaceNotFound);
        }

        var document = new Document(
            Guid.NewGuid(),
            command.WorkspaceId,
            normalizedName,
            normalizedOriginalFileName,
            normalizedContentType,
            command.SizeInBytes);

        await documentRepository.AddAsync(
            document,
            cancellationToken);

        var result = new CreateDocumentResult(
            document.Id,
            document.Name,
            document.CreatedAtUtc);

        return CreateDocumentResponse.Success(result);
    }
}