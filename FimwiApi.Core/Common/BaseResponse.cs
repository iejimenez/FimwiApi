using System.Text.Json.Serialization;

namespace FimwiApi.Core.Common;

public class BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }

    public static BaseResponse SuccessResponse(string? message = null, object? data = null)
    {
        return new BaseResponse
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static BaseResponse ErrorResponse(string message, List<string>? errors = null)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
} 