
namespace DocuMind.Domain.Documents;

public sealed class DocumentChunk
{

    private DocumentChunk()
    {
    }

    public DocumentChunk(
        Guid id,
        Guid documentId,
        int index,
        string content)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Document chunk ID cannot be empty.",
                nameof(id));
        }

        if (documentId == Guid.Empty)
        {
            throw new ArgumentException(
                "Document ID cannot be empty.",
                nameof(documentId));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(index),
                index,
                "Chunk index must be non-negative.");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException(
                "Content cannot be null or whitespace.",
                nameof(content));
        }

        Id = id;
        DocumentId = documentId;
        Index = index;
        Content = content.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid DocumentId { get; private set; }
    public int Index { get; private set; }
    public string Content { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }
}