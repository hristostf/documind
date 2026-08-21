namespace DocuMind.Application.Documents.Search;

public sealed record SearchDocumentsQuery(
    Guid OrganizationId,
    Guid WorkspaceId,
    string Query,
    int Limit = 5);