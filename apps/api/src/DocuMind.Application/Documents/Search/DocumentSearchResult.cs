namespace DocuMind.Application.Documents.Search;

public sealed record DocumentSearchResult(
    Guid DocumentChunkId,
    Guid DocumentId,
    string Content,
    double Distance);