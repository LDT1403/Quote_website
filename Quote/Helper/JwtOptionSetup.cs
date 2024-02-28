using Microsoft.Extensions.Options;
using Quote.Modal;

namespace Quote.Helper
{
    public class JwtOptionSetup : IConfigureOptions<Jwtoptions>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtOptionSetup(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        public void Configure(Jwtoptions options)
        {
           _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
