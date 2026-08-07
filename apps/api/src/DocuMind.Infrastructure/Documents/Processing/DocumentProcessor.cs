using DocuMind.Application.Documents;
using DocuMind.Application.Documents.Processing;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class DocumentProcessor(
    IDocumentRepository documentRepository)
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

        document.StartProcessing();

        await documentRepository.UpdateAsync(
            document,
            cancellationToken);

        await Task.Delay(
            TimeSpan.FromSeconds(2),
            cancellationToken);

        document.MarkAsReady();

        await documentRepository.UpdateAsync(
            document,
            cancellationToken);
    }
}