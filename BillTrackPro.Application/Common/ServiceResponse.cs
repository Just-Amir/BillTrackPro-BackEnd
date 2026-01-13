namespace BillTrackPro.Application.Common;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();

    public static ServiceResponse<T> Ok(T data, string message = "Operation successful")
    {
        return new ServiceResponse<T> { Data = data, Message = message };
    }

    public static ServiceResponse<T> Fail(string message, List<string>? errors = null)
    {
        return new ServiceResponse<T> 
        { 
            Success = false, 
            Message = message, 
            Errors = errors ?? new List<string>() 
        };
    }
}
