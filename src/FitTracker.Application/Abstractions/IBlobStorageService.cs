namespace FitTracker.Application.Abstractions
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<string> GetDownloadUrlAsync(string videoUrl, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default);
    }
}
