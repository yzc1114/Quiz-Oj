using System.Runtime.InteropServices;
using System; 
using System.Reflection; 
using System.Globalization; 
using Microsoft.CSharp;
using System.CodeDom; 
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using quiz_oj.Configs.Exceptions;
using quiz_oj.Entities.OJ;
using quiz_oj.IndependentNS;


namespace quiz_oj.DynamicCodeExecutor
{
    
    
    public class Compiler
    {
        // [DllImport("libTest", CallingConvention = CallingConvention.Cdecl)]
        // public static extern void Hello();
        //
        // [DllImport("libTest", CallingConvention = CallingConvention.Cdecl)]
        // public static extern int TestAdd(int a, int b);
        //
        // public void Test()
        // {
        //     Hello();
        //     Console.WriteLine(TestAdd(1, 2));
        // }

        private readonly ConcurrentDictionary<string, AssemblyLoadContext> _contexts =
            new ConcurrentDictionary<string, AssemblyLoadContext>();
        
        private static class CompilationFactory
        {

            private static readonly string DotnetCoreDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            private static readonly CSharpCompilationOptions Options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            private static readonly MetadataReference[] References =
            {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ListNode).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(DotnetCoreDirectory, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(DotnetCoreDirectory, "netstandard.dll")),
                MetadataReference.CreateFromFile(Path.Combine(DotnetCoreDirectory, "System.Runtime.dll"))
            };

            public static CSharpCompilation Create(string uuid, string code)
            {
                return CSharpCompilation.Create(uuid, options: Options, references: References,
                    syntaxTrees: new[]
                    {
                        CSharpSyntaxTree.ParseText(code)
                    });
            }
        }

        public async Task<Assembly> Compile(string uuid, string code)
        {
            var compilation = await Task.Run(() => CompilationFactory.Create(uuid,@"using System; using quiz_oj.IndependentNS;" + code));
            await using var ms = new MemoryStream();
            var emitRes = compilation.Emit(ms);
            if (emitRes.Success)
            {
                ms.Seek(0, SeekOrigin.Begin);
                var context = _contexts.GetOrAdd(uuid, s => new AssemblyLoadContext(s, true));
                var a = context.LoadFromStream(ms);
                return a;
            }
            var builder = new StringBuilder();
            builder.AppendLine("Compilation was failed.");
            foreach (var resDiagnostic in emitRes.Diagnostics)
            {
                builder.AppendLine(resDiagnostic.ToString()); Console.WriteLine(resDiagnostic.ToString());
            }
            
            builder.AppendLine("Please check your code...");
            throw new UserException(builder.ToString());
        }

        public void UnloadAssembly(string uuid)
        {
            _contexts.Remove(uuid, out var ctx);
            ctx?.Unload();
        }
    }

    public static class AssemblyExtensions
    {
        public static object CreateInstanceByClassName(this Assembly assembly, string className)
        {
            var instance = assembly.CreateInstance(className);
            if (instance == null)
            {
                throw new UserException($"Couldn't find class {className} in Code, Please Check Your Code.");
            }

            return instance;
        }

        public static MethodInfo GetMethodByNames(this Assembly assembly, string className, string methodName)
        {
            var cls = assembly.GetType(className);
            if (cls == null)
            {
                throw new UserException($"Couldn't find class {className} in Code, Please Check Your Code.");
            }
            var met = cls.GetMethod(methodName);
            if (met == null)
            {
                throw new UserException($"Couldn't find method {methodName} in Code, Please Check Your Code.");
            }
            return met;
        }
    }
}