namespace DocuMind.Infrastructure.AI;

internal sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string EmbeddingModel { get; init; } =
        "text-embedding-3-small";

    public string AnswerModel { get; init; } = "gpt-5-mini";

    public string ApiKey { get; init; } = string.Empty;
}