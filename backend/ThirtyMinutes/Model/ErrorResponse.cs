namespace ThirtyMinutes.Model
{
    public class ErrorResponse
    {
        public int Code;
        public string Message;

        public ErrorResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}