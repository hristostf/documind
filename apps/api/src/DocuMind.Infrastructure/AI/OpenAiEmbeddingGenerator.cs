using DocuMind.Application.Documents.Processing;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;

namespace DocuMind.Infrastructure.AI;

internal sealed class OpenAiEmbeddingGenerator
    : IEmbeddingGenerator
{
    private readonly EmbeddingClient _client;

    public OpenAiEmbeddingGenerator(
        IOptions<OpenAiOptions> options)
    {
        var settings = options.Value;

        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API key is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.EmbeddingModel))
        {
            throw new InvalidOperationException(
                "OpenAI embedding model is not configured.");
        }

        _client = new EmbeddingClient(
            settings.EmbeddingModel,
            settings.ApiKey);
    }

    public async Task<IReadOnlyList<float>> GenerateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(
                "Text cannot be null or whitespace.",
                nameof(text));
        }

        var embedding = await _client.GenerateEmbeddingAsync(
            text,
            cancellationToken: cancellationToken);

        return embedding.Value
            .ToFloats()
            .ToArray();
    }
}