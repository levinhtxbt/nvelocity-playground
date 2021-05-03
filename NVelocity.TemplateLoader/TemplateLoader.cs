using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NVelocity.Tool;

namespace NVelocity.TemplateLoader
{
    public class TemplateLoader : ITemplateLoader
    {
        private readonly IConfiguration _configuration;
        private readonly HttpProcess _http;

        public TemplateLoader(IConfiguration configuration, HttpProcess http)
        {
            _configuration = configuration;
            _http = http;
        }

        public string GetTemplate(string name)
        {

            var url = _configuration["TemplateUrl"] ?? "";
            url = Path.Combine(url, name);

            var jsonResponse =_http.Get(url).GetAwaiter().GetResult();
            var jsonData = JObject.Parse(jsonResponse);

            return jsonData["html"]?.ToString() ?? "";
        }

        public async Task<string> GetTemplateAsync(string url)
        {
            return string.Empty;
        }
    }
}
