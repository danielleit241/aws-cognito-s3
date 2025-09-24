using Amazon.S3;
using Amazon.S3.Model;
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

        public async Task DeleteFileAsync(string key, string type)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{type}/{key}"
                };
                await _s3Client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(string key, string type)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{type}/{key}"
            };
            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string type, Stream fileStream)
        {
            var key = $"{Guid.NewGuid()}_{file.FileName}";
            var putRequest = new PutObjectRequest
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

        public async Task<string> PreSignedUrlAsync(string key, string type)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = $"{type}/{key}",
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Verb = HttpVerb.GET
                };

                var url = await _s3Client.GetPreSignedURLAsync(request);
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating presigned URL: {ex.Message}", ex);
            }
        }
    }
}
