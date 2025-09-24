namespace cognito_s3.Services
{
    public interface IAwsS3
    {
        Task<string> UploadFileAsync(IFormFile file, string type, Stream fileStream);
        Task<Stream> DownloadFileAsync(string key, string type);
        Task DeleteFileAsync(string key, string type);
        Task<string> PreSignedUrlAsync(string key, string type);
    }
}
