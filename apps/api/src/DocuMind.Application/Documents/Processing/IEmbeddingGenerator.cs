namespace DocuMind.Application.Documents.Processing;
    
public interface IEmbeddingGenerator
{
    Task<IReadOnlyList<float>> GenerateAsync(
        string text,
        CancellationToken cancellationToken = default);
}