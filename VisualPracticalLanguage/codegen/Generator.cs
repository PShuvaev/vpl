using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
	public class Generator
	{
		private TextWriter output;

		public Generator (TextWriter output)
		{
			this.output = output;
		}
		
		public void Generate(IFunctionCall call)
		{
			// если функция - бинарная операция
			if (call.function.isBinOperation) {
				output.Write ("(");
				Generate(call.arguments.First());
				output.Write (call.function.name);
				Generate(call.arguments[1]);
				output.Write (")");
				return;
			}

			output.Write (call.function.name);
			output.Write ("(");

			var args = call.arguments.EmptyIfNull ();
			var firstArg = args.FirstOrDefault ();

			if (firstArg != null) {
				Generate (firstArg);

				foreach (var arg in args.Skip (1)) {
					output.Write (", ");
					Generate (arg);
				}
			}
			output.Write (")");
		}

		public void Generate(IConstExpression expression)
		{
			output.Write (expression.constValue);
		}

		public void Generate(IVariable variable)
		{
			output.Write (variable.varName);
		}

		public void Generate(IIfStatement statement)
		{
			output.Write ("if(");
			Generate (statement.condition);
			output.Write ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			output.Write ("}");
		}

		public void Generate(IWhileStatement statement)
		{
			output.Write ("while(");
			Generate (statement.condition);
			output.Write ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			output.Write ("}");
		}

		public void Generate(ISetVariableStatement statement)
		{
			output.Write (statement.variable.varName);
			output.Write ("=");
			Generate (statement.expression);
			output.Write (";");
		}
		
		public void Generate(IFunCallStatement statement)
		{
			Generate (statement.functionCall);
			output.Write (";");
		}

		public void Generate(IReturnStatement statement)
		{
			output.Write ("return ");
			Generate (statement.expression);
			output.Write (";");
		}

		public void Generate(IFunctionDefinition definition)
		{
			// сигнатура метода
			output.Write ("public ");
			if (definition.isReturnVoid) {
				output.Write ("void");
			} else {
				output.Write ("dynamic");
			}
			output.Write (" ");
			output.Write (definition.name);
			output.Write ("(");
			definition.arguments.IterSep (@variable => {
				output.Write ("dynamic ");
				output.Write (@variable.varName);
			}, _ => {output.Write (", ");});
			output.Write ("){");

			// блок переменных
			if (!definition.variables.Empty ()) {
				output.Write ("dynamic ");
				definition.variables.IterSep (@variable => {
					output.Write (@variable.varName);
				}, _ => {output.Write (", ");});
				output.Write (";");
			}

			// тело функции
			foreach (var statement in definition.statements) {
				Generate (statement);
			}
			
			output.Write ("}");
		}
		
		/// <summary>
		/// Динамическая диспетчеризация для Statement-выражений
		/// </summary>
		public void Generate(IStatement statement)
		{
			if (statement is IIfStatement)
				Generate (statement as IIfStatement);
			if (statement is IWhileStatement)
				Generate (statement as IWhileStatement);
			if (statement is ISetVariableStatement)
				Generate (statement as ISetVariableStatement);
			if (statement is IReturnStatement)
				Generate (statement as IReturnStatement);
			if (statement is IFunCallStatement)
				Generate (statement as IFunCallStatement);
		}

		/// <summary>
		/// Динамическая диспетчеризация для Expression-выражений
		/// </summary>
		public void Generate(IExpression expression)
		{
			if (expression is IVariable)
				Generate (expression as IVariable);
			if (expression is IConstExpression)
				Generate (expression as IConstExpression);
			if (expression is IFunctionCall)
				Generate (expression as IFunctionCall);
		}
	}


	public class GeneratorTest{
		
		public dynamic fibb1(dynamic n){
			dynamic a,b,c;
			a = 0; b = 1;
			while (n > 0) {
				c = a;
				a = b;
				b = c + b;
				n = n - 1;
			}
			return a;
		}

		public void Test(){
			var writer = new StringWriter ();
			var generator = new Generator (writer);

			var fibbFunc = new FunctionDefinition {name = "fibb"};
			var nVar = fibbFunc.AddArgument("n");
			var aVar = fibbFunc.AddVariable("a");
			var bVar = fibbFunc.AddVariable("b");
			var cVar = fibbFunc.AddVariable("c");
			fibbFunc.AddStatement (new SetVariableStatement {
				variable = aVar,
				expression = new ConstExpression { constValue = "0" }
			});
			fibbFunc.AddStatement (new SetVariableStatement {
				variable = bVar,
				expression = new ConstExpression { constValue = "1" }
			});
			fibbFunc.AddStatement (new WhileStatement {
				condition = new FunctionCall {
					function = new FunctionDeclaration {
						name = ">",
						isBinOperation = true,
						isReturnVoid = false
					},
					arguments = new List<IExpression>{
						nVar, new ConstExpression{constValue = "0"}
					}
				},
				statements = new List<IStatement>{
					new SetVariableStatement {
						variable = cVar,
						expression = aVar
					},
					new SetVariableStatement {
						variable = aVar,
						expression = bVar
					},
					new SetVariableStatement {
						variable = bVar,
						expression = new FunctionCall {
							function = new FunctionDeclaration {
								name = "+",
								isBinOperation = true,
								isReturnVoid = true
							},
							arguments = new List<IExpression> {
								cVar, bVar
							}
						}
					},
					new SetVariableStatement {
						variable = nVar,
						expression = new FunctionCall {
							function = new FunctionDeclaration {
								name = "-",
								isBinOperation = true,
								isReturnVoid = true
							},
							arguments = new List<IExpression> {
								nVar, new ConstExpression {
									constValue = "1"
								}
							}
						}
					},
				}
			});

			generator.Generate (fibbFunc);
			System.Console.WriteLine (writer.ToString());
		}
	}
}

