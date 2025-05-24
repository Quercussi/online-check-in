namespace OnlineCheckIn.Application.DTOs.Responses;

public class BaseHttpResponse<T>
{
    public string? ResponseCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public void SetResponse(T? data, string responseCode = "200", string? message = null)
    {
        this.ResponseCode = responseCode;
        this.Data = data;
        this.Message = message;
    }
}

public class PaginatedDataResponse<T>
{
    public List<T>? Items { get; set; }
    public int Total { get; set; }
}