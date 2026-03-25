using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using FitTracker.Application.Abstractions;
using FitTracker.Domain.Shared;
using Google.Apis.Auth.OAuth2;


namespace FitTracker.Infra.Services
{
    public sealed class BlobStorageService : IBlobStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly GCloudOptions _gCloudOptions;

        public BlobStorageService(IOptions<GCloudOptions> gCloudOptions)
        {
            _gCloudOptions = gCloudOptions.Value;
            _storageClient = StorageClient.Create();
        }

        public async Task<string> UploadFileAsync(
            Stream fileStream, 
            string fileName, 
            string contentType, 
            CancellationToken cancellationToken = default)
        {
            var sanitizedName = fileName.Replace(" ", "_");
            var objectName = $"videos/{Guid.NewGuid()}_{sanitizedName}";
            
            await _storageClient.UploadObjectAsync(
                _gCloudOptions.BucketName,
                objectName,
                contentType,
                fileStream,
                cancellationToken: cancellationToken);

            return $"https://storage.googleapis.com/{_gCloudOptions.BucketName}/{objectName}";
        }

        public async Task<string> GetDownloadUrlAsync(string videoUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(videoUrl)) return string.Empty;

            var prefix = $"https://storage.googleapis.com/{_gCloudOptions.BucketName}/";
            if (!videoUrl.StartsWith(prefix))
            {
                return videoUrl;
            }

            var objectName = videoUrl.Substring(prefix.Length);

            try 
            {
                UrlSigner signer;
                if (!string.IsNullOrEmpty(_gCloudOptions.CredentialPath) && File.Exists(_gCloudOptions.CredentialPath))
                {
                    using var stream = new FileStream(_gCloudOptions.CredentialPath, FileMode.Open, FileAccess.Read);
                    var credential = GoogleCredential.FromStream(stream);
                    signer = UrlSigner.FromCredential(credential);
                }
                else 
                {
                    // Attempt to get from ambient credentials (standard in Cloud Run/Local env variables)
                    var credential = await GoogleCredential.GetApplicationDefaultAsync();
                    
                    // Note: UrlSigner requires a service account credential to sign.
                    signer = UrlSigner.FromCredential(credential);
                }

                return await signer.SignAsync(
                    _gCloudOptions.BucketName,
                    objectName,
                    TimeSpan.FromHours(1),
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                // If signing fails (e.g. no service account), fallback to public URL to avoid breaking the app
                // but log this as it's a security/config issue
                Console.WriteLine($"[BlobStorageService] ERROR: Failed to sign URL for {objectName}. Reason: {ex.Message}");
                return videoUrl;
            }
        }

        public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            // Extract object name from URL
            // URL format: https://storage.googleapis.com/bucket-name/object-name
            var prefix = $"https://storage.googleapis.com/{_gCloudOptions.BucketName}/";
            if (!fileUrl.StartsWith(prefix)) return;

            var objectName = fileUrl.Substring(prefix.Length);

            await _storageClient.DeleteObjectAsync(
                _gCloudOptions.BucketName,
                objectName,
                cancellationToken: cancellationToken);
        }
    }
}
