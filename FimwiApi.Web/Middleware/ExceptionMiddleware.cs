using System.Net;
using System.Text.Json;
using FimwiApi.Core.Common;
using FimwiApi.Core.Exceptions;

namespace FimwiApi.Web.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        
        var errorResponse = new BaseResponse
        {
            Success = false,
            Message = "An error occurred while processing your request."
        };

        switch (exception)
        {
            case AppException appException:
                response.StatusCode = appException.StatusCode;
                errorResponse.Message = appException.Message;
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "You are not authorized to access this resource.";
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = "The requested resource was not found.";
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (_env.IsDevelopment())
                {
                    errorResponse.Errors = new List<string> { exception.Message, exception.StackTrace };
                }
                break;
        }

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
} 