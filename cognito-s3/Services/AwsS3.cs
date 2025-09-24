using Amazon.S3;
using Microsoft.Extensions.Options;

namespace cognito_s3.Services
{
    public class AwsS3 : IAwsS3
    {
        private readonly IOptions<S3Settings> _options;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AwsS3(IOptions<S3Settings> options, IAmazonS3 s3Client)
        {
            _options = options;
            _s3Client = s3Client;
            _bucketName = options.Value.BucketName;
        }

        public Task DeleteFileAsync(string bucketName, string keyName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFileAsync(string bucketName, string keyName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListBucketsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFileAsync(IFormFile file, string type, Stream fileStream)
        {
            var key = $"{Guid.NewGuid()}_{file.FileName}";
            var putRequest = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{type}/{key}",
                InputStream = fileStream,
                ContentType = file.ContentType,
                Metadata =
                {
                    ["file-name"] = file.FileName,
                    ["uploaded-at"] = DateTime.UtcNow.ToString("o")
                }
            };
            await _s3Client.PutObjectAsync(putRequest);
            var fileUrl = $"https://{_bucketName}.s3.{_options.Value.Region}.amazonaws.com/{type}/{key}";
            return fileUrl;
        }
    }
}
