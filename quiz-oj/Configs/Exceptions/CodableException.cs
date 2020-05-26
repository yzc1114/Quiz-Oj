using System;

namespace quiz_oj.Configs.Exceptions
{
    public class CodableException : Exception
    {
        public int Code { get; set; }
        
        public CodableException(string msg) : base(msg)
        {
            Code = -1;
        }

        public CodableException(string msg, int code) : base(msg)
        {
            Code = code;
        }
    }
}