using Newtonsoft.Json.Linq;
using NVelocity.Service.Constant;
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

        public TemplateProcess()
        {
            _http = new HttpProcess();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(environmentName))
                _env = "development";
            else
                _env = environmentName.ToLower();
        }

        public async Task<string> GetTemplate(string name)
        {
            var url = "";

            switch (_env)
            {
                case "development":
                    url = TemplateUrlConstant.Dev;
                    break;
                case "staging":
                    url = TemplateUrlConstant.Staging;
                    break;
                case "production":
                    url = TemplateUrlConstant.Production;
                    break;
                default:
                    url = TemplateUrlConstant.Dev;
                    break;
            }

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
