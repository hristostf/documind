using DocuMind.Application.Documents.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DocuMind.Infrastructure.Documents.Processing;

internal sealed class DocumentProcessingWorker(
    IDocumentProcessingQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<DocumentProcessingWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var documentId = await queue.DequeueAsync(
                    stoppingToken);

                using var scope = scopeFactory.CreateScope();

                var processor = scope.ServiceProvider
                    .GetRequiredService<IDocumentProcessor>();

                await processor.ProcessAsync(
                    documentId,
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Unexpected error while processing a document.");
            }
        }
    }
}