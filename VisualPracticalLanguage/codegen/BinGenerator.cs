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

		static IEnumerable<IFunctionDefinition> AddReturnNullToFuns(IEnumerable<IFunctionDefinition> funs){
			var copyFuns = funs.Select (x => new FunctionDefinition {
				name = x.name,
				arguments = x.arguments.EmptyIfNull().ToList(),
				variables = x.variables.EmptyIfNull().ToList(),
				isBinOperation = x.isBinOperation,
				statements = x.statements.EmptyIfNull().ToList()
			}).ToList();
			foreach (var fun in copyFuns) {
				fun.statements.Add (new JustCode("if(0==0){return null;}"));
			}
			return copyFuns;
		}

		string GetSrc(){
			var output = new StringWriter ();
			var copyNs = new Namespace {
				importedDlls = ns.importedDlls,
				namespaceName = ns.namespaceName,
				functions = AddReturnNullToFuns(ns.functions).ToList().With(_ => {
					if(_.Select(x => x.name).Contains(Const.MainFunName)){
						_.Add(new JustCode("public static void Main(string[] args)"+
						                   string.Format("{{new {0}().{1}();}}", ns.namespaceName, Const.MainFunName)));
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
			var dlls = new[] { "mscorlib.dll", "System.Core.dll", "Microsoft.CSharp.dll" }.Concat(
				ns.importedDlls.EmptyIfNull ().Select (x => x + ".dll"));

            var tempOutputFileName = Path.GetTempFileName() + ".exe";
            var parameters = new CompilerParameters(dlls.ToArray(), tempOutputFileName, false){
				GenerateExecutable = true,
				GenerateInMemory = false,
				TreatWarningsAsErrors = false,
				MainClass = ns.namespaceName,
				CompilerOptions = "/target:winexe"
			};

			var src = GetSrc ();
			Logger.Log (src);

			var results = csc.CompileAssemblyFromSource(parameters, src);
            var errors = results.Errors.Cast<CompilerError>().ToList();

            if (!errors.Empty())
            {
                MessageBox.Show(
                    string.Join("\r\n", errors.Select(x => x.ErrorText)), "Ошибка");
                return null;
            }

            File.Copy(tempOutputFileName, outputAssemblyName, true);

			return results.CompiledAssembly;
		}

		public void Execute(){
			Assembly assembly = Build ();
            if (assembly == null) return;

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

