using Customer.Model.Common;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace NotificationApi.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    var errorContext = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorContext != null)
                    {
                        await context.Response.WriteAsJsonAsync(new Response<string>()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = new List<string>() { errorContext.Error.Message },
                            Data = errorContext.Error.StackTrace,
                            RequestTime = DateTime.Now
                        });
                    }
                });
            });
        }
    }
}
