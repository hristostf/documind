

namespace DocuMind.Application.Documents.CreateDocument;


public sealed record CreateDocumentCommand(
    Guid OrganizationId,
    Guid WorkspaceId, 
    string Name,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes,
    Stream Content);