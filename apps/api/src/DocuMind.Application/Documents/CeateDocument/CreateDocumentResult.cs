namespace DocuMind.Application.Documents.CreateDocument;

public sealed record CreateDocumentResult(
    Guid Id,
    string Name,
    DateTime CreatedAtUtc);