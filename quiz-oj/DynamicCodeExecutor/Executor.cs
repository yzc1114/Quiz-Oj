using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using quiz_oj.Configs.Exceptions;

namespace quiz_oj.DynamicCodeExecutor
{
    public class Executor
    {
        public async Task<int> Execute(ExecutableData executableData)
        {
            return await Task.Run(() =>
            {
                if (executableData.Params.Length != executableData.ExpectedResults.Length)
                {
                    throw new NonUserException($"Params Length Doesn't equal to ExpectedResults Length. Params Length: {executableData.Params.Length}, ExpectedResult Length {executableData.ExpectedResults.Length}");
                }
                try
                {
                    var successes = 0;
                    for (var i = 0; i < executableData.Params.Length; i++)
                    {
                        var ps = executableData.Params[i];
                        var ep = executableData.ExpectedResults[i];
                        var res = executableData.Method.Invoke(executableData.Instance, ps);
                        if (res == null && ep == null
                        || res != null && res.Equals(ep))
                        {
                            successes += 1;
                        }
                    }

                    return successes;
                }
                catch (Exception e)
                {
                    throw new UserException("Runtime Exception Encountered:\n" + e.Message);
                }
            });
        }
    }
}