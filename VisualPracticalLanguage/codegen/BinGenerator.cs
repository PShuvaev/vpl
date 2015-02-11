using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace VisualPracticalLanguage
{
	public class BinGenerator
	{
		public BinGenerator ()
		{
		}

		
		public void Run()
		{
			var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
			var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "foo.exe", false){
				GenerateExecutable = true,
				OutputAssembly = "/home/ps/projects/VisualPracticalLanguage/out.exe",
				GenerateInMemory = false,
				TreatWarningsAsErrors = false
			};

			CompilerResults results = csc.CompileAssemblyFromSource(parameters,
			@"using System.Linq;
			using System;
            class Program {
              public static void Main(string[] args) {
                var q = from i in Enumerable.Range(1,100)
                          where i % 2 == 0
                          select i;
                System.Console.WriteLine(String.Join("", "", q.ToArray()));
              }
            }");

			results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));

			Assembly assembly = results.CompiledAssembly;


			Type     type     = assembly.GetType("Program");
			var      obj      = Activator.CreateInstance(type);

			// Alternately you could get the MethodInfo for the TestRunner.Run method
			type.InvokeMember("Main",
			                  BindingFlags.Default | BindingFlags.InvokeMethod, 
			                  null,
			                  obj,
			                  new object[]{null});
		}
	}
}

