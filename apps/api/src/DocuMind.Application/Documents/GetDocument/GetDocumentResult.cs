namespace DocuMind.Application.Documents.GetDocument;

public sealed record GetDocumentResult(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes,
    string Status,
    DateTime CreatedAtUtc);