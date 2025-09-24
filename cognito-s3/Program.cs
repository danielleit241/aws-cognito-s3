using Amazon.S3;
using cognito_s3;
using cognito_s3.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtBearerConfigurationOptions>();

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings"));
builder.Services.AddSingleton<IAmazonS3>(o =>
{
    var s3Settings = o.GetRequiredService<IOptions<S3Settings>>().Value;
    var config = new AmazonS3Config
    {
        RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(s3Settings.Region)
    };

    return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, config);
});

builder.Services.AddScoped<IAwsS3, AwsS3>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("api/claims", (ClaimsPrincipal claims) =>
{
    return claims.Claims.Select(c => new { c.Type, c.Value }).ToArray();
}).RequireAuthorization();

app.MapPost("api/images", async ([FromForm] IFormFile file, IAwsS3 awsS3) =>
{
    if (file.Length == 0)
    {
        return Results.BadRequest("File is empty.");
    }
    if (file.Length > 5 * 1024 * 1024)
    {
        return Results.BadRequest("File size exceeds 5 MB.");
    }
    using var stream = file.OpenReadStream();

    var fileUrl = await awsS3.UploadFileAsync(file, "images", stream);

    return Results.Ok(new { FileUrl = fileUrl });
})
    .RequireAuthorization()
    .DisableAntiforgery();

app.Run();