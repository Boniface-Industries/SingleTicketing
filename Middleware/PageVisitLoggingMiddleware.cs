using Microsoft.AspNetCore.Http;
using SingleTicketing.Services;
using System.Threading.Tasks;
using SingleTicketing.Data;
using SingleTicketing.Models;
using SingleTicketing.Services;
namespace SingleTicketing.Middleware
{
    public class PageVisitLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public PageVisitLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IActivityLogService activityLogService)
        {
            // Retrieve the user's ID from the session
            var userId = context.Session.GetInt32("LoggedInUserId");

            if (userId.HasValue)
            {
                // Get the URL of the page being visited
                string pageUrl = context.Request.Path;

                // Capture the user's IP address
                string ipAddress = context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";

                // Log the page visit
                await activityLogService.LogPageVisitAsync(userId.Value, pageUrl, ipAddress);
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
