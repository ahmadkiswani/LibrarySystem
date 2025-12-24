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

        public async Task InvokeAsync(
            HttpContext context,
            ILogEventPublisher publisher)
        {
            var correlationId = Guid.NewGuid().ToString();
            var serviceName = "LibrarySystem.API";

            context.Items["CorrelationId"] = correlationId;

         
            context.Request.EnableBuffering();

            string requestBody = string.Empty;
            if (context.Request.Body.CanRead)
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    leaveOpen: true);

                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var requestText =
                $"{context.Request.Method} {context.Request.Path} {requestBody}";

           
            var originalBody = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                responseBody.Position = 0;
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();

             
                bool isLogicalError =
                    context.Response.StatusCode >= 400 ||
                    responseText.Contains("\"success\":false");

                if (isLogicalError)
                {
                    await publisher.PublishExceptionAsync(new LogExceptionDto
                    {
                        CorrelationId = correlationId,
                        Time = DateTime.UtcNow,
                        ServiceName = serviceName,
                        Request = requestText,
                        Response = responseText,
                        Message = "Logical / Business error"
                    });
                }
                else
                {
                    await publisher.PublishRequestAsync(new LogRequestDto
                    {
                        CorrelationId = correlationId,
                        Time = DateTime.UtcNow,
                        ServiceName = serviceName,
                        Request = requestText
                    });

                    await publisher.PublishResponseAsync(new LogResponseDto
                    {
                        CorrelationId = correlationId,
                        Time = DateTime.UtcNow,
                        ServiceName = serviceName,
                        Response = responseText
                    });
                }

             
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBody);
            }
            catch (Exception ex)
            {
             
                await publisher.PublishExceptionAsync(new LogExceptionDto
                {
                    CorrelationId = correlationId,
                    Time = DateTime.UtcNow,
                    ServiceName = serviceName,
                    Request = requestText,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });

                context.Response.Body = originalBody;
                throw;
            }
        }
    }
}
