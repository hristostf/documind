
namespace DocuMind.Application.Documents.ListDocuments;

public sealed record ListDocumentItem(
    Guid Id,
    string Name,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes,
    string Status,
    DateTime CreatedAtUtc);