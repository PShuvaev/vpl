using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using VisualPracticalLanguage.Interface;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace VisualPracticalLanguage
{
	public class BinGenerator
	{
		INamespace ns;

		public BinGenerator (INamespace ns)
		{
			this.ns = ns;
		}

		string GetSrc(){
			var output = new StringWriter ();
			var copyNs = new Namespace {
				importedDlls = ns.importedDlls,
				namespaceName = ns.namespaceName,
				functions = ns.functions.EmptyIfNull().ToList().With(_ => {
					if(_.Select(x => x.name).Contains("Старт")){
						_.Add(new JustCode(@"
							public static void Main(string[] args){new $ClassName$().Старт();}
						".Replace("$ClassName$", ns.namespaceName)));
					}
				})
			};
			new Generator (output).Generate (copyNs);
			return output.ToString ();
		}

		public Assembly Build()
		{
			var outputAssemblyName = ns.namespaceName + ".exe";
			File.Delete(outputAssemblyName);

			var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			var dlls = new[] { "mscorlib.dll", "System.Core.dll" }.Concat(
				ns.importedDlls.EmptyIfNull ().Select (x => x + ".dll"));
			var parameters = new CompilerParameters(dlls.ToArray(), outputAssemblyName, false){
				GenerateExecutable = false,
				GenerateInMemory = true,
				TreatWarningsAsErrors = false,
				MainClass = "test",
				CompilerOptions = "/target:winexe"
			};
			Logger.Log (GetSrc());
			CompilerResults results = csc.CompileAssemblyFromSource(parameters, GetSrc());

			results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));

			return results.CompiledAssembly;
		}

		public void Execute(){
			Assembly assembly = Build ();
			Type type = assembly.GetTypes ().FirstOrDefault (t => t.IsClass);

			try {
				if(IsUnix()){
					Process.Start ("mono", ns.namespaceName + ".exe");
				} else {
					Process.Start (ns.namespaceName + ".exe");
				}
			} catch(Exception e){
				MessageBox.Show("Ошибка запуска. Проверьте программу на ошибки.");
				Logger.Log (e.StackTrace);
			}
		}

		static bool IsUnix ()
		{
			int p = (int)Environment.OSVersion.Platform;
			return p == 4 || p == 128;
		}
	}
}

