using DocuMind.Application.Documents;
using DocuMind.Application.Documents.Processing;
using DocuMind.Domain.Documents;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class DocumentProcessor(
    IDocumentRepository documentRepository,
    IDocumentTextExtractor textExtractor,
    ITextChunker textChunker,
    IDocumentChunkRepository documentChunkRepository,
    IEmbeddingGenerator embeddingGenerator,
    IDocumentChunkEmbeddingRepository embeddingRepository)
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

            var chunkContents = textChunker.Chunk(text);

            var chunks = chunkContents
                .Select((content, index) =>
                    new DocumentChunk(
                        Guid.NewGuid(),
                        documentId,
                        index,
                        content))
                .ToList();

            await documentChunkRepository.AddRangeAsync(
                chunks,
                cancellationToken);

            foreach (var chunk in chunks)
            {
                var embedding = await embeddingGenerator.GenerateAsync(
                    chunk.Content,
                    cancellationToken);

                await embeddingRepository.AddAsync(
                    chunk.Id,
                    embedding,
                    cancellationToken);
            }


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