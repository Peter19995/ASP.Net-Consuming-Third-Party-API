using ConsumingThirdPartyAPI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace ConsumingThirdPartyAPI.App_Start
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            System.Reflection.Assembly thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Example On Consuming Third Party - API");
                    c.PrettyPrint();
                    c.OperationFilter<HeaderParameter>();
                })

                .EnableSwaggerUi(c =>
                {
                    c.DocExpansion(DocExpansion.List);
                    c.DocumentTitle("Consuming Third Party - API V1");
                });
        }
    }
}