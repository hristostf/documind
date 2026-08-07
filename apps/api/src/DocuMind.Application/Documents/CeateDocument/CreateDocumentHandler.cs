

using DocuMind.Application.Workspaces;
using DocuMind.Domain.Documents;
using DocuMind.Application.Storage;
using DocuMind.Application.Documents.Processing;
namespace DocuMind.Application.Documents.CreateDocument;


public sealed class CreateDocumentHandler(
    IWorkspaceRepository workspaceRepository,
    IDocumentRepository documentRepository,
    IFileStorage fileStorage,
    IDocumentProcessingQueue processingQueue)
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

        var storageKey = await fileStorage.SaveAsync(
            command.Content,
            normalizedOriginalFileName,
            cancellationToken);

        var document = new Document(
            Guid.NewGuid(),
            command.WorkspaceId,
            normalizedName,
            normalizedOriginalFileName,
            normalizedContentType,
            command.SizeInBytes,
            storageKey);

        await documentRepository.AddAsync(
            document,
            cancellationToken);

        await processingQueue.EnqueueAsync(
            document.Id,
            cancellationToken);

        var result = new CreateDocumentResult(
            document.Id,
            document.Name,
            document.CreatedAtUtc);

        return CreateDocumentResponse.Success(result);
    }
}