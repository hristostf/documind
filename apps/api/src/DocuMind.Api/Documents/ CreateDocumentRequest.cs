namespace DocuMind.Api.Documents;

public sealed record CreateDocumentRequest(
    string Name,
    string OriginalFileName, 
    string ContentType, 
    long SizeInBytes);