using System;
using System.Linq;
using System.Security.Claims;
using LibraryManagement.Middlewares;

namespace LibraryManagement.Helpers
{
    public static class Extensions
    {
        public static int GetLoggedInUserId(this ClaimsPrincipal claims)
        {
            var userIdString = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(userIdString))
                throw new Exception("Session expired. Please logout and login again");

            var result = int.TryParse(userIdString, out var userId);

            if (!result)
            {
                throw new Exception("Invalid credentials. Please logout and login again");

            }
            return userId;
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app) =>
           app.UseMiddleware<ExceptionHandlingMiddleware>();

        public static async Task InvokeAsync<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs eventArgs)
        {
            if (handler == null)
                return;

            var invocationList = handler.GetInvocationList();

            var tasks = invocationList
                .OfType<EventHandler<TEventArgs>>()
                .Select(d => Task.Run(() => d(sender, eventArgs)));

            await Task.WhenAll(tasks);
        }
    }
}