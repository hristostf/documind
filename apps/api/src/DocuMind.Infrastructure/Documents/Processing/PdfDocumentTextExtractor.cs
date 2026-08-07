using System.Text;
using DocuMind.Application.Documents.Processing;
using DocuMind.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using UglyToad.PdfPig;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class PdfDocumentTextExtractor(
    IOptions<StorageOptions> options)
    : IDocumentTextExtractor
{
    private readonly string _rootPath =
        options.Value.RootPath;

    public Task<string> ExtractTextAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(
            _rootPath,
            storageKey);

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(
                "Document file was not found.",
                fullPath);
        }

        var builder = new StringBuilder();

        using var pdf = PdfDocument.Open(fullPath);

        foreach (var page in pdf.GetPages())
        {
            cancellationToken.ThrowIfCancellationRequested();

            builder.AppendLine(page.Text);
            builder.AppendLine();
        }

        return Task.FromResult(builder.ToString());
    }
}