namespace quiz_oj.Configs.Middlewares
{
    public class ResponseFormat
    {
        public int Code { get; }
        public string Message { get; }
        public object Data { get; }

        public ResponseFormat(int code, string message, object data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
}