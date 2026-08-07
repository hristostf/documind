

namespace DocuMind.Application.Documents.CreateDocument;

public sealed record CreateDocumentResponse
{
    private CreateDocumentResponse(
        CreateDocumentResult? document,
        CreateDocumentError error)
    {
        Document = document;
        Error = error;
    }

    public CreateDocumentResult? Document { get; }

    public CreateDocumentError Error { get; }

    public bool IsSuccess =>
        Error == CreateDocumentError.None;

    public static CreateDocumentResponse Success(
        CreateDocumentResult document)
    {
        ArgumentNullException.ThrowIfNull(document);

        return new CreateDocumentResponse(
            document,
            CreateDocumentError.None);
    }

    public static CreateDocumentResponse Failure(
        CreateDocumentError error)
    {
        if (error == CreateDocumentError.None)
        {
            throw new ArgumentException(
                "A failure response must contain an error.",
                nameof(error));
        }

        return new CreateDocumentResponse(
            document: null,
            error);
    }
}