using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public HomeController(IWebHostEnvironment env, ImageProcessor image, IServiceProvider provider)
        {
            _env = env;
            _image = image;
            _velocityEngine = new VelocityEngine(provider);
            _velocityEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
            FILE_RESOURCE_LOADER_PATH = Path.Combine(
                _env.ContentRootPath /*AppDomain.CurrentDomain.BaseDirectory*/,
                "Templates");

        }

        [HttpGet("{templateName}")]
        public IActionResult Index(string templateName)
        {
            var htmlTemplate = GetHtmlTemplate(templateName);

            GeVariables(htmlTemplate);


            ExtendedProperties extendedProperties = new ExtendedProperties();
            extendedProperties.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, FILE_RESOURCE_LOADER_PATH);
            _velocityEngine.Init(extendedProperties);


            VelocityContext velocityContext = new VelocityContext();

            velocityContext.Put("image", _image);
            velocityContext.Put("subheading", "subheading");
            velocityContext.Put("heading", "heading");
            velocityContext.Put("companyName", "Zinl");
            velocityContext.Put("couponHelper", new CouponHelper());
            velocityContext.Put("couponsIncluded", true);

            var coupons = new List<Coupon>()
            {
                new Coupon()
                {
                    Code = "Ella",
                    CouponExpires = DateTime.Now.AddDays(5),
                    CouponLinkType = "ABC",
                    CouponType = "ABC",
                    Name = "Ella",
                    SubTitle = "Hello Ella",
                    Details = "ABC XYZ"
                },
                new Coupon()
                {
                    Code = "Ella2",
                    CouponExpires = DateTime.Now.AddDays(5),
                    CouponLinkType = "ABC",
                    CouponType = "ABC",
                    Name = "Ella2",
                    SubTitle = "Hello Ella2",
                    Details = "ABC XYZ"
                }
            };
            velocityContext.Put("coupons", coupons);

            var amenities = new List<Amenities>()
            {
                new Amenities()
                {
                    Name = "Coffee",
                    ImageHTTPUrl =
                        "https://img.favpng.com/7/14/17/coffee-cup-icon-food-icon-basic-icons-icon-png-favpng-f3XBwvvxg0RnubteZpA2iCgM4.jpg"
                },
                new Amenities()
                {
                    Name = "Lounge",
                    ImageHTTPUrl = "https://image.flaticon.com/icons/png/128/2819/2819936.png"
                }
            };

            velocityContext.Put("amenities", amenities);



            return EvaluateHtml(velocityContext, htmlTemplate);
        }

        private void GeVariables(string htmlTemplate)
        {
            string pattern = @"(\$!?[a-zA-Z]+\w*)(?!.*\1)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);

            var regexResult = rgx.Matches(htmlTemplate);

            System.IO.File.WriteAllText(Path.Combine(FILE_RESOURCE_LOADER_PATH, "variables.txt"),
                string.Join("\n", regexResult.Select(x => x.Value)));


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

    public class Amenities
    {
        public string Name { get; set; }

        public string ImageHTTPUrl { get; set; }
    }

    public class Coupon
    {
        public string Name { get; set; }

        public string SubTitle { get; set; }

        public string Code { get; set; }

        public string CouponType { get; set; }

        public string Details { get; set; }

        public DateTime CouponExpires { get; set; }

        public string CouponLinkType { get; set; }

        public DateTime GetExpirationDate(object a)
        {
            return DateTime.UtcNow;
        }


    }

    public class CouponHelper
    {
        public string GetPrintUrl(Coupon c)
        {
            return $"http://google.com";
        }

        public bool ShowPrintLink(Coupon c)
        {
            return true;
        }

        public string GetCouponLink(Coupon c, string title)
        {
            return $"http://google.com";
        }
    }
}
