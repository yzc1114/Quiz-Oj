using System;
using System.Diagnostics.CodeAnalysis;

namespace quiz_oj.Configs.Exceptions
{
    public class NonUserException : CodableException
    {
        public NonUserException(string message) : base(message) { }
        public NonUserException(string message, int code) : base(message, code) { }
    }
}