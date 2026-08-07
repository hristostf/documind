
using DocuMind.Application.Storage;
using DocuMind.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace DocuMind.Infrastructure.Storage;

internal sealed class LocalFileStorage(
    IOptions<StorageOptions> options)
    : IFileStorage
{
    private readonly string _rootPath =
        !string.IsNullOrWhiteSpace(options.Value.RootPath)
            ? options.Value.RootPath
            : throw new InvalidOperationException(
                "Storage root path is not configured.");

    public async Task<string> SaveAsync(
        Stream content,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_rootPath);

        var extension = Path.GetExtension(fileName);

        var storedFileName =
            $"{Guid.NewGuid()}{extension}";

        var fullPath = Path.Combine(
            _rootPath,
            storedFileName);

        await using var fileStream = new FileStream(
            fullPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await content.CopyToAsync(
            fileStream,
            cancellationToken);

        return storedFileName;
    }
}