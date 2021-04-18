using System.IO;
using System.Text;
using Commons.Collections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocityPlayground.Web.Processor;

namespace NVelocityPlayground.Web.Controllers
{
    [Route("home")]
    public class HomeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ImageProcessor _image;
        private readonly VelocityEngine _velocityEngine;
        private readonly string FILE_RESOURCE_LOADER_PATH;

        public HomeController(IWebHostEnvironment env, ImageProcessor image)
        {
            _env = env;
            _image = image;
            _velocityEngine = new VelocityEngine();
            _velocityEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
            FILE_RESOURCE_LOADER_PATH = Path.Combine(
                _env.ContentRootPath /*AppDomain.CurrentDomain.BaseDirectory*/,
                "Templates");
            
        }

        [HttpGet("{templateName}")]
        public IActionResult Index(string templateName)
        {
            var htmlTemplate = GetHtmlTemplate(templateName);

            ExtendedProperties extendedProperties = new ExtendedProperties();
            extendedProperties.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            _velocityEngine.Init(extendedProperties);


            VelocityContext velocityContext = new VelocityContext();

            velocityContext.Put("image", _image);
            velocityContext.Put("subheading", "subheading");
            velocityContext.Put("heading", "heading");
            velocityContext.Put("companyName", "Zinl");






            return EvaluateHtml(velocityContext, htmlTemplate);
        }

        private IActionResult EvaluateHtml(VelocityContext velocityContext, string htmlTemplate)
        {
            StringWriter stringWriter = new StringWriter();

            _velocityEngine.Evaluate(
                velocityContext,
                stringWriter,
                "",
                htmlTemplate);

            var htmlResult = stringWriter.GetStringBuilder().ToString();

            return Content(htmlResult, "text/html", Encoding.UTF8);
        }

        private string GetHtmlTemplate(string templateName)
        {
            var template = Path.Combine(FILE_RESOURCE_LOADER_PATH,
                templateName);

            var htmlTemplate = System.IO.File.ReadAllText(template);
            return htmlTemplate;
        }
        
        
    }

    public class Website
    {
        public string Url { get; set; }
    }
}
