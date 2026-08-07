using DocuMind.Application.Documents;
using DocuMind.Application.Documents.Processing;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class DocumentProcessor(
    IDocumentRepository documentRepository,
      IDocumentTextExtractor textExtractor)
    : IDocumentProcessor
{
    public async Task ProcessAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {

        var document = await documentRepository.GetByIdAsync(
                documentId,
                cancellationToken);

        if (document is null)
        {
            return;
        }

        try
        {
            document.StartProcessing();

            await documentRepository.UpdateAsync(
                document,
                cancellationToken);

            var text = await textExtractor.ExtractTextAsync(
                    document.StorageKey,
                    cancellationToken);

                Console.WriteLine(
                    $"Extracted {text.Length} characters from document {document.Id}");


            document.MarkAsReady();

            await documentRepository.UpdateAsync(
                document,
                cancellationToken);

        } catch
        {
            document.MarkAsFailed();

            await documentRepository.UpdateAsync(
                document,
                cancellationToken);

            throw;
        }
    }
}