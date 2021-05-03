using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NVelocity;
using NVelocity.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVelocityPlayground.Web.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly VelocityEngine _velocityEngine;

        public TestingController( IServiceProvider provider)
        {
            _velocityEngine = new VelocityEngine(provider);
            _velocityEngine.Init();
        }

        public IActionResult Index()
        {
            var html = "#template('aaaaaa')";

            var context = new VelocityContext();

            context.Put("name", "Phuc mlem mlem");

            return EvaluateHtml(context, html);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetTemplate(string name)
        {
            return Ok(new
            {
                Html = "<h1>Hello $name</h1>"
            });
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
    }
}
