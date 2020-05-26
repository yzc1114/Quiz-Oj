using System.Reflection;

namespace quiz_oj.DynamicCodeExecutor
{
    public class ExecutableData
    {
        public object Instance { get; set; }
        public MethodInfo Method { get; set; }
        public object[][] Params { get; set; }
        public object[] ExpectedResults { get; set; }
        
    }
}