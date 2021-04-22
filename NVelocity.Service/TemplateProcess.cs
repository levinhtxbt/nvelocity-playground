using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NVelocity.Service
{
    public class TemplateProcess
    {
        private readonly HttpProcess _http;
        private readonly string _env;
        private readonly IConfiguration _configuration;

        public TemplateProcess(IConfiguration configuration, HttpProcess http)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<string> GetTemplate(string name)
        {
            var url = "";

            url = _configuration["TemplateUrl"];

            url = Path.Combine(url, name);

            var jsonResponse = await _http.Get(url);
            var jsonData = JObject.Parse(jsonResponse);

            var html = "";

            if (jsonData != null)
                html = jsonData["html"]?.ToString() ?? "";

            return html;
        }
    }
}
