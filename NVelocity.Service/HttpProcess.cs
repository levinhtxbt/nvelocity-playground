using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NVelocity.Service
{
    public class HttpProcess
    {
        private readonly HttpClient _client;

        public HttpProcess()
        {
            _client = new HttpClient();
        }

        public async Task<string> Get(string url)
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            throw new System.Exception("Something went wrong");
        }
    }
}
