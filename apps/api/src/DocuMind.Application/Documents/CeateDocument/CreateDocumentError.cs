namespace DocuMind.Application.Documents.CreateDocument;

public enum CreateDocumentError
{
    None = 0,
    NameRequired,
    OriginalNameRequired,
    ContentTypeRequired,
    InvalidSizeInBytes,
    WorkspaceNotFound

}