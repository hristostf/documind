namespace DocuMind.Infrastructure.AI;

internal sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string EmbeddingModel { get; init; } =
        "text-embedding-3-small";

    public string ApiKey { get; init; } = string.Empty;
}