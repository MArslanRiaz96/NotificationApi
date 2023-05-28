using Customer.Data.Context;
using Customer.Data.Models;
using System.Security.Claims;

namespace NotificationApi.Extentions
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context != null && context.Request != null && context.Request.Headers.Count > 0 && context.Request.Headers.ContainsKey("Logging"))
                {
                    string loggingValue = context.Request.Headers["Logging"].FirstOrDefault();
                    await Logs(context, loggingValue.ToString());
                }
            }
            catch (Exception ex) { }
            finally
            {
                await _next(context);
            }
        }
        private async Task Logs(HttpContext context, string action)
        {
            string email = context.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var request = context.Request;
            var _logging = new Logging
            {
                Action = action,
                ActionDetail = request.Headers["Logging"].FirstOrDefault().ToString(),
                App_Id = 1,
                Status = context.Response.StatusCode.ToString(),
                ComapnyId = "",
                Email = email,
                Request_By = context.User.GetUserId(),
                Request_Date = DateTime.UtcNow,
                Request_Type = context.Request.Method,
            };
            await LoggingInSQL(_logging, context);
        }
        private async Task LoggingInSQL(Logging logging, HttpContext _context)
        {
            //var dbContext = _context.RequestServices.GetRequiredService<ApplicationDbContext>();
            //dbContext.Loggings.Add(logging);
            //await dbContext.SaveChangesAsync();
        }
    }
}
