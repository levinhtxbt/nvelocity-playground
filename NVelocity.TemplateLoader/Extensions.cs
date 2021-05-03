using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NVelocity.Tool;

namespace NVelocity.TemplateLoader
{
    public static class Extensions
    {
        public static NVelocityBuilder  AddTemplateLoader(this NVelocityBuilder velocityBuilder)
        {
            velocityBuilder.Services.AddScoped<HttpProcess>();
            velocityBuilder.Services.AddScoped<ITemplateLoader, TemplateLoader>();
            return velocityBuilder;
        }
    }
}
