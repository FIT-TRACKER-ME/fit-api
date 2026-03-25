namespace FitTracker.Domain.Shared
{
    public class GCloudOptions
    {
        public string ProjectId { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string? CredentialPath { get; set; }
    }
}
