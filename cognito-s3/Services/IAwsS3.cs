namespace cognito_s3.Services
{
    public interface IAwsS3
    {
        Task<List<string>> ListBucketsAsync();
        Task<string> UploadFileAsync(IFormFile file, string type, Stream fileStream);
        Task<Stream> DownloadFileAsync(string bucketName, string keyName);
        Task DeleteFileAsync(string bucketName, string keyName);
    }
}
