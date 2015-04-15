using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	// TODO: Разместить методы Generate по каждому классу?
	public class Generator
	{
		private TextWriter output;

		public Generator (TextWriter output)
		{
			this.output = output;
		}

		private void Spit(params object[] values) {
			foreach(var val in values){
				output.Write (" ");
				output.Write (val ?? "null");
				output.Write (" ");
			}
		}

		public void Generate(INamespace @namespace)
		{
			foreach (var ns in @namespace.importedDlls.EmptyIfNull())
				Spit ("using", ns, ";");
			Spit ("public class", @namespace.namespaceName, "{");
				foreach (var fun in @namespace.functions)
					Generate (fun);
			Spit ("}");
		}

		public void Generate(IFunctionCall call)
		{
			// если функция - бинарная операция
			if (call.function.isBinOperation) {
				Spit ("(");
				Generate(call.arguments.First());
				Spit (call.function.name);
				Generate(call.arguments[1]);
				Spit (")");
				return;
			}

			Spit (call.function.name, "(");

			var args = call.arguments.EmptyIfNull ().ToList();

			if (!args.Empty()) {
				Generate (args.FirstOrDefault());

				foreach (var arg in args.Skip (1)) {
					Spit (",");
					Generate (arg);
				}
			}
			Spit (")");
		}

		public void Generate(IConstExpression expression)
		{
			if (expression == null || expression.constValue == null) {
				Spit ("null");
			} else {
				var val = expression.constValue;
				Spit (val is string ? '"' + val.ToString () + '"' : val.ToString ());
			}
		}

		public void Generate(IVariableRef variableRef)
		{
			Spit (variableRef.varName);
		}

		public void Generate(IIfStatement statement)
		{
			Spit ("if(");
			Generate (statement.condition);
			Spit ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			Spit ("}");
		}

		public void Generate(IWhileStatement statement)
		{
			Spit ("while(");
			Generate (statement.condition);
			Spit ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			Spit ("}");
		}

		public void Generate(ISetVariableStatement statement)
		{
			Spit (statement.variableRef.varName, "=");
			Generate (statement.expression);
			Spit (";");
		}
		
		public void Generate(IFunCallStatement statement)
		{
			Generate (statement.functionCall);
			Spit (";");
		}

		public void Generate(IReturnStatement statement)
		{
			Spit ("return");
			Generate (statement.expression);
			Spit (";");
		}

		public void Generate(IFunctionDefinition funDef)
		{
			if(funDef is JustCode){
				Spit ((funDef as JustCode).code);
				return;
			}

			// сигнатура метода
			Spit ("public", "dynamic", funDef.name, "(");
			funDef.arguments.IterSep (@variable => {
				Spit ("dynamic", @variable.varName);
			}, _ => {Spit (",");});
			Spit ("){");

			// блок переменных
			if (!funDef.variables.Empty ()) {
				Spit ("dynamic");
				funDef.variables.IterSep (@variable => {
					Spit (@variable.varName);
				}, _ => {Spit (",");});
				Spit (";");
			}

			// тело функции
			foreach (var statement in funDef.statements) {
				Generate (statement);
			}
			
			Spit ("}");
		}
		
		/// <summary>
		/// Динамическая диспетчеризация для Statement-выражений
		/// </summary>
		public void Generate(IStatement statement)
		{
			if (statement == null) {
				Spit ("null");
				return;
			}
			if (statement is JustCode) {
				Spit ((statement as JustCode).code);
				return;
			}
			if (statement is IIfStatement){
				Generate (statement as IIfStatement);
				return;
			}
			if (statement is IWhileStatement){
				Generate (statement as IWhileStatement);
				return;
			}
			if (statement is ISetVariableStatement){
				Generate (statement as ISetVariableStatement);
				return;
			}
			if (statement is IReturnStatement){
				Generate (statement as IReturnStatement);
				return;
			}
			if (statement is IFunCallStatement){
				Generate (statement as IFunCallStatement);
				return;
			}
		}

		/// <summary>
		/// Динамическая диспетчеризация для Expression-выражений
		/// </summary>
		public void Generate(IExpression expression)
		{
			if (expression == null) {
				Spit ("null");
				return;
			}
			if (expression is IVariableRef){
				Generate (expression as IVariableRef);
				return;
			}
			if (expression is IConstExpression){
				Generate (expression as IConstExpression);
				return;
			}
			if (expression is IFunctionCall){
				Generate (expression as IFunctionCall);
				return;
			}
			if (expression is IReturnStatement){
				Generate (expression as IReturnStatement);
				return;
			}
		}
	}
}