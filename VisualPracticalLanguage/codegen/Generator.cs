using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public class Generator
	{
		private TextWriter output;

		public Generator (TextWriter output)
		{
			this.output = output;
		}
		
		public void Generate(FunctionCall call)
		{
			// если функция - бинарная операция
			if (call.function.IsBinOperation) {
				output.Write ("(");
				Generate(call.arguments.First());
				output.Write (call.function.name);
				Generate(call.arguments[1]);
				output.Write (")");
				return;
			}

			output.Write (call.function.name);
			output.Write ("(");
			call.arguments.EmptyIfNull ().Aggregate (0, (_, expression) => {
				Generate(expression);
				output.Write (", ");
				return 0;
			});
			output.Write (")");
		}

		public void Generate(ConstExpression expression)
		{
			output.Write (expression.value);
		}

		public void Generate(Variable variable)
		{
			output.Write (variable.name);
		}

		public void Generate(IfStatement statement)
		{
			output.Write ("if(");
			Generate (statement.condition);
			output.Write ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			output.Write ("}");
		}

		public void Generate(WhileStatement statement)
		{
			output.Write ("while(");
			Generate (statement.condition);
			output.Write ("){");
			foreach (var st in statement.statements.EmptyIfNull()) {
				Generate (st);
			}
			output.Write ("}");
		}

		public void Generate(SetVariableStatement statement)
		{
			output.Write (statement.variable.name);
			output.Write ("=");
			Generate (statement.expression);
			output.Write (";");
		}
		
		public void Generate(FunCallStatement statement)
		{
			Generate (statement.functionCall);
			output.Write (";");
		}

		public void Generate(ReturnStatement statement)
		{
			output.Write ("return ");
			Generate (statement.expression);
			output.Write (";");
		}

		public void Generate(FunctionDefinition definition)
		{
			// сигнатура метода
			output.Write ("public ");
			if (definition.ReturnVoid) {
				output.Write ("void");
			} else {
				output.Write ("dynamic");
			}
			output.Write (" ");
			output.Write (definition.name);
			output.Write ("(");
			definition.arguments.IterSep (@variable => {
				output.Write ("dynamic ");
				output.Write (@variable.name);
			}, _ => {output.Write (", ");});
			output.Write ("){");

			// блок переменных
			if (!definition.variables.Empty ()) {
				output.Write ("dynamic ");
				definition.variables.IterSep (@variable => {
					output.Write (@variable.name);
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
			var @switch = new Dictionary<Type, Action> {
				{ typeof(IfStatement), () => {Generate(statement as IfStatement);}},
				{ typeof(WhileStatement), () => {Generate(statement as WhileStatement);}},
				{ typeof(SetVariableStatement), () => {Generate(statement as SetVariableStatement);} },
				{ typeof(ReturnStatement), () => {Generate(statement as ReturnStatement);} },
				{ typeof(FunCallStatement), () => {Generate(statement as FunCallStatement);} },
			};

			@switch[statement.GetType()]();
		}

		/// <summary>
		/// Динамическая диспетчеризация для Expression-выражений
		/// </summary>
		public void Generate(IExpression expression)
		{
			var @switch = new Dictionary<Type, Action> {
				{ typeof(Variable), () => {Generate(expression as Variable);}},
				{ typeof(ConstExpression), () => {Generate(expression as ConstExpression);} },
				{ typeof(FunctionCall), () => {Generate(expression as FunctionCall);} },
			};

			@switch[expression.GetType()]();
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
				expression = new ConstExpression { value = "0" }
			});
			fibbFunc.AddStatement (new SetVariableStatement {
				variable = bVar,
				expression = new ConstExpression { value = "1" }
			});
			fibbFunc.AddStatement (new WhileStatement {
				condition = new FunctionCall {
					function = new FunctionDeclaration {
						name = ">",
						IsBinOperation = true,
						ReturnVoid = false
					},
					arguments = new List<IExpression>{
						nVar, new ConstExpression{value = "0"}
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
								IsBinOperation = true,
								ReturnVoid = true
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
								IsBinOperation = true,
								ReturnVoid = true
							},
							arguments = new List<IExpression> {
								nVar, new ConstExpression {
									value = "1"
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

