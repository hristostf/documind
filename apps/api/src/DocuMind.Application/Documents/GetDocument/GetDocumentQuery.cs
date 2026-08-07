namespace DocuMind.Application.Documents.GetDocument;

public sealed record GetDocumentQuery(
    Guid OrganizationId,
    Guid WorkspaceId,
    Guid DocumentId);