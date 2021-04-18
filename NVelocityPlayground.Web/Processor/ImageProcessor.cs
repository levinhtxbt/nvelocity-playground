using System;
using Microsoft.AspNetCore.Hosting;

namespace NVelocityPlayground.Web.Processor
{
    public class ImageProcessor
    {
        private readonly IWebHostEnvironment _env;

        public ImageProcessor(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetUrl(string key)
        {
            return $"https://m1-data.s3.ap-southeast-2.amazonaws.com/dealer/67fab45c-9789-41f0-aeb0-983bfe79ba7e-dealer.jpg";
        }

        public bool OverrideExists(string key)
        {
            return Convert.ToBoolean(new Random().Next(0, 1));
        }
    }
}