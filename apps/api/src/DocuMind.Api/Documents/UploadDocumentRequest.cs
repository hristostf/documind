namespace DocuMind.Api.Documents;

public sealed class UploadDocumentRequest
{
    public IFormFile File { get; init; } = default!;

    public string? Name { get; init; }
}