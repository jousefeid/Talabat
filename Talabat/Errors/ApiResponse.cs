namespace Talabat.Errors
{
    public class ApiResponse
    {
        public int  StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefuaktMessageForStatusCode(statusCode);
        }

        private string? GetDefuaktMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You are not authorized",
                404 => "Resource not found",
                500 => "Inteernal Server Error",
                _ => null
            };
        }
    }
}
