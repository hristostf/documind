
namespace DocuMind.Domain.Documents;

public sealed class Document
{
    private Document()
    {
    }

    public Document(
        Guid id,
        Guid workspaceId,
        string name,
        string originalFileName,
        string contentType,
        long sizeInBytes,
        string storageKey)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException(
                "Document ID cannot be empty.",
                nameof(id));
        }

        if(workspaceId == Guid.Empty)
        {
            throw new ArgumentException(
                "Workspace ID cannot be empty.",
                nameof(workspaceId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Document name cannot be null or whitespace.",
                nameof(name));
        }


        if (string.IsNullOrWhiteSpace(originalFileName))
        {
            throw new ArgumentException(
                "Original file name cannot be null or whitespace.",
                nameof(originalFileName));
        }

        if (string.IsNullOrWhiteSpace(contentType))
        {
            throw new ArgumentException(
                "Content type cannot be null or whitespace.",
                nameof(contentType));
        }

        if(sizeInBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sizeInBytes),
                sizeInBytes,
                "Size in bytes must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new ArgumentException(
                "Storage key cannot be null or whitespace.",
                nameof(storageKey));
        }

        Id = id;
        WorkspaceId = workspaceId;
        Name = name.Trim();
        OriginalFileName = originalFileName.Trim();
        ContentType = contentType.Trim();
        SizeInBytes = sizeInBytes;
        StorageKey = storageKey.Trim();
        Status = DocumentStatus.Uploaded;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid WorkspaceId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string OriginalFileName { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long SizeInBytes { get; private set; }

    public DocumentStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public string StorageKey { get; private set; } = string.Empty;


    public void StartProcessing()
    {
        if (Status != DocumentStatus.Uploaded)
        {
            throw new InvalidOperationException(
                $"Cannot start processing document in {Status} status.");
        }

        Status = DocumentStatus.Processing;
    }

    public void MarkAsReady()
    {
        if (Status != DocumentStatus.Processing)
        {
            throw new InvalidOperationException(
                $"Cannot mark document as ready from {Status} status.");
        }

        Status = DocumentStatus.Ready;
    }

    public void MarkAsFailed()
    {
        if (Status != DocumentStatus.Processing)
        {
            throw new InvalidOperationException(
                $"Cannot mark document as failed from {Status} status.");
        }

        Status = DocumentStatus.Failed;
    }
}