namespace DocuMind.Application.Documents.Ask;

public sealed record AskDocumentsQuery(
    Guid OrganizationId,
    Guid WorkspaceId,
    string Question,
    int Limit = 5);