using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Reflection.Metadata;

namespace MotoShop.Api.Swagger
{
    /// <summary>
    /// Добавление StatusCode 400 и 200 ко всем контроллерам и методам контроллеров, у которых есть атрибут [HttpResponses]
    /// Если данный StatusCode уже есть, например из XML комментариев - заново StatusCode добавлен не будет
    /// </summary>
    public class HttpResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                   .Union(context.MethodInfo.GetCustomAttributes(true))
                                   .OfType<HttpResponsesAttribute>();

            if (authAttributes.Any())
            {
                if (!operation.Responses.ContainsKey("400"))
                {
                    operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
                }

                if (!operation.Responses.ContainsKey("200"))
                {
                    operation.Responses.Add("200", new OpenApiResponse { Description = "Ok" });
                }
            }
        }
    }
}
