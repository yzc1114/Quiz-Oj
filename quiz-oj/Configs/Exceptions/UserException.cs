using System;
using MyExceptionLibrary.ExceptionLibrary;
namespace quiz_oj.Configs.Exceptions
{
    public class UserException : CodableException
    {
        public UserException(string msg) : base(msg) {}
        public UserException(string msg, int code) : base(msg, code) { }
    }
}