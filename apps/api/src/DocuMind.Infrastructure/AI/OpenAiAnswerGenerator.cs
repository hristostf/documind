using System.Text;
using DocuMind.Application.Documents.Ask;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace DocuMind.Infrastructure.AI;

internal sealed class OpenAiAnswerGenerator
    : IAnswerGenerator
{
    private readonly ChatClient _client;

    public OpenAiAnswerGenerator(
        IOptions<OpenAiOptions> options)
    {
        var settings = options.Value;

        if (string.IsNullOrWhiteSpace(settings.ApiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API key is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.AnswerModel))
        {
            throw new InvalidOperationException(
                "OpenAI answer model is not configured.");
        }

        _client = new ChatClient(
            settings.AnswerModel,
            settings.ApiKey);
    }

    public async Task<string> GenerateAsync(
        string question,
        IReadOnlyList<string> contextChunks,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            throw new ArgumentException(
                "Question cannot be null or whitespace.",
                nameof(question));
        }

        if (contextChunks.Count == 0)
        {
            throw new ArgumentException(
                "At least one context chunk is required.",
                nameof(contextChunks));
        }

        var contextBuilder = new StringBuilder();

        for (var i = 0; i < contextChunks.Count; i++)
        {
            contextBuilder.AppendLine(
                $"[Context {i + 1}]");

            contextBuilder.AppendLine(
                contextChunks[i]);

            contextBuilder.AppendLine();
        }

        var messages = new ChatMessage[]
        {
            new SystemChatMessage(
                """
                You are a document question-answering assistant.

                Answer the user's question using only the provided context.

                If the answer cannot be found in the context,
                say that you do not have enough information.

                Keep the answer concise and factual.
                """),

            new UserChatMessage(
                $"""
                Context:

                {contextBuilder}

                Question:

                {question}
                """)
        };

        var completion = await _client.CompleteChatAsync(
            messages,
            cancellationToken: cancellationToken);

        return completion.Value.Content[0].Text;
    }
}