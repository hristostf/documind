


namespace DocuMind.Application.Documents.ListDocuments;

public sealed record ListDocumentsQuery(
    Guid OrganizationId,
    Guid WorkspaceId);