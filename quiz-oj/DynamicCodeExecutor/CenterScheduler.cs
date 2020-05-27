using System;
using System.Threading.Tasks;
using quiz_oj.Entities.OJ;

namespace quiz_oj.DynamicCodeExecutor
{
    public class CenterScheduler
    {
        private readonly Compiler _compiler;
        private readonly Executor _executor;

        public CenterScheduler(Compiler compiler, Executor executor)
        {
            _compiler = compiler;
            _executor = executor;
        }

        public async Task<OjResult> PendCode(string uuid, string code, OjTestCaseSet ojTestCaseSet)
        {
            try
            {
                var a = await _compiler.Compile(uuid, code);
                var instance = a.CreateInstanceByClassName("Solution");
                var method = a.GetMethodByNames("Solution", ojTestCaseSet.MethodName);

                var successes = await _executor.Execute(new ExecutableData
                {
                    Instance = instance,
                    Method = method,
                    ExpectedResults = ojTestCaseSet.CastExpectedReturnValues(),
                    Params = ojTestCaseSet.CastParamsToObjects(),
                });
                var cases = ojTestCaseSet.Params.Length;
                var pass = cases == successes;
                var successRate = 100 * successes * 1.0 / cases;
                return new OjResult
                {
                    Pass = pass,
                    Pending = false,
                    SuccessRate = successRate,
                    Failed = false,
                };
            }
            catch (Exception e)
            {
                return new OjResult
                {
                    Pass = false,
                    Failed = true,
                    Pending = false,
                    Info = e.Message
                };
            }
            finally
            {
                _compiler.UnloadAssembly(uuid);
            }
        }
    }
}