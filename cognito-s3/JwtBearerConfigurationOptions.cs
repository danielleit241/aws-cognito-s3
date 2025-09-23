using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace cognito_s3
{
    public class JwtBearerConfigurationOptions(IConfiguration configuration)
        : IConfigureNamedOptions<JwtBearerOptions>
    {
        private const string ConfigurationSectionName = "JwtBearer";

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
