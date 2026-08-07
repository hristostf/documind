

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocuMind.Application.Storage;

public interface IFileStorage
{
    Task<string> SaveAsync(
        Stream stream,
        string fileName,
        CancellationToken cancellationToken = default);
}