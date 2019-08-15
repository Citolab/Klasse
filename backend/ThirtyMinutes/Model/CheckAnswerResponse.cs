namespace ThirtyMinutes.Model
{
    public class CheckAnswerResponse
    {
        public int Code;
        public string Message;
        public int TotalPenaltySecondsApplied;
        public int RemainingSeconds;

        public CheckAnswerResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}