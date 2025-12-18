using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Interfaces;
using System.Text;

namespace LibrarySystem.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogService logService)
        {
            var correlationId = Guid.NewGuid().ToString();
            var serviceName = "LibrarySystem.API";

            context.Items["CorrelationId"] = correlationId;

            //Req
            context.Request.EnableBuffering();
            string requestBody = string.Empty;

            if (context.Request.Body.CanRead)

            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    leaveOpen: true
                );

                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            await logService.LogRequestAsync(new LogRequestDto
            {
                CorrelationId = correlationId,
                ServiceName = serviceName,
                Request = $"{context.Request.Method} {context.Request.Path} {requestBody}"
            });

            //Res
            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();

                bool isLogicalError =
                    context.Response.StatusCode >= 400 ||
                    responseText.Contains("\"success\":false");

                if (isLogicalError)
                {
                    await logService.LogExceptionAsync(new LogExceptionDto
                    {
                        CorrelationId = correlationId,
                        ServiceName = serviceName,
                        Message = "Logical / Business Error",
                        StackTrace = responseText
                    });
                }

                await logService.LogResponseAsync(new LogResponseDto
                {
                    CorrelationId = correlationId,
                    ServiceName = serviceName,
                    Response = responseText
                });

                var bytes = Encoding.UTF8.GetBytes(responseText);
                context.Response.Body = originalBody;
                await context.Response.Body.WriteAsync(bytes);
            }
            catch (Exception ex)
            {
                await logService.LogExceptionAsync(new LogExceptionDto
                {
                    CorrelationId = correlationId,
                    ServiceName = serviceName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace ?? string.Empty
                });

                context.Response.Body = originalBody;
                throw;
            }
        }
    }
}
