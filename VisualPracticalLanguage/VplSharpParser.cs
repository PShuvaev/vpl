using System;
using Sprache;
using System.Linq;
using System.Collections.Generic;
using VisualPracticalLanguage.Interface;
using System.IO;

namespace VisualPracticalLanguage
{
	// TODO: приоритет операций в выражениях определяется исключительно скобками 
	public class VplSharpParser
	{
		static Parser<string> IdentP = Parse.Letter.AtLeastOnce().Text().Token();

		static Parser<string> TokenP(string s){
			return Parse.String (s).Text().Token();
		}

		static Parser<string> QuotedTextP =
			(from _ in Parse.Char('"')
			 from txt in Parse.CharExcept('"').Many().Text()
			 from __ in Parse.Char('"')
			 select txt).Token();


		public static Parser<TC> BracketP<TC>(Parser<TC> content){
			return (
				from _ in TokenP("(")
				from res in content
				from __ in TokenP(")")
				select res);
		}

		static Parser<IFunctionCall> FunCallP = (
			from funName in IdentP
			from _ in TokenP("(")
			from arguments in (
			from __ in TokenP(")")
			select new List<IExpression>()
			).Or(
			from args in Parse.DelimitedBy(Parse.Ref(() => ExpressionP), TokenP(","))
			from __ in TokenP(")")
			select args.ToList()
			)
			select new FunctionCall{
			arguments = arguments,
			function = new FunctionDeclaration{
				name = funName,
				isBinOperation = false
			}
		});

		static Parser<IVariable> VariableP = 
			from varName in IdentP
				select new Variable { varName = varName };

		static Parser<IConstExpression> ConstP = 
			(from str in QuotedTextP
			 select new ConstExpression { constValue = '"' + str + '"' })
				.Or(from dec in Parse.Decimal.Token()
				    select new ConstExpression { constValue = dec });

		static Parser<IExpression> ExpressionP = 
			from firstExpr in
				BracketP<IExpression> (Parse.Ref (() => ExpressionP))
				.Or (Parse.Ref (() => ConstP))
				.Or (Parse.Ref (() => FunCallP))
				.Or (Parse.Ref (() => VariableP))
				from operationPart in Parse.Optional(
					from ____ in Parse.WhiteSpace.Many()
					from op in Parse.Chars (new char[] { '+', '-', '*', '/', '>', '<'})
					from v2 in Parse.Ref(() => ExpressionP)
					select new FunctionCall {
					arguments = new List<IExpression> {v2},
					function = new FunctionDeclaration{
						isBinOperation = true,
						name = op+""
					}
				})
				select (
					operationPart.IsEmpty 
					? firstExpr 
					: operationPart.Get().With(fun => {
					fun.arguments.Insert(0, firstExpr);
				}));


		static Parser<IReturnStatement> ReturnStatementP =
			from _ in TokenP ("return")
			from mExpr in Parse.Optional (Parse.Ref(() => ExpressionP))
			from __ in TokenP (";")
				select new ReturnStatement {
			expression = mExpr.IsEmpty ? null : mExpr.Get()
		};

		static Parser<ISetVariableStatement> SetStatementP =
			from variable in VariableP
				from _ in TokenP("=")
				from expr in Parse.Ref(() => ExpressionP)
				from __ in TokenP(";")
				select new SetVariableStatement {
			variable = variable,
			expression = expr
		};

		static Parser<IFunCallStatement> CallProcStatementP =
			from fcall in Parse.Ref(() => FunCallP)
			from _ in TokenP(";")
			select new FunCallStatement {
				functionCall = fcall
			};

		static Parser<IStatement> StatementP =
			condStatementP ("if", (expr, stmts) => new IfStatement {condition = expr, statements = stmts})
				.Or (condStatementP ("while", (expr, stmts) => new WhileStatement {condition = expr, statements = stmts}))
				.Or (SetStatementP.Select(x => (IStatement)x))
				.Or (CallProcStatementP.Select(x => (IStatement)x))
				.Or (ReturnStatementP);

		static Parser<ICondStatement> condStatementP(string condType, Func<IExpression, IList<IStatement>, ICondStatement> constructor){ 
			return
				from _ in TokenP (condType)
					from __ in TokenP ("(")
					from expr in Parse.Ref (() => ExpressionP)
					from ___ in TokenP (")")
					from ____ in TokenP ("{")
					from statements in Parse.Ref (() => StatementP.Many ())
					from _____ in TokenP ("}")
					select constructor (expr, statements.ToList ());
		}

		static Parser<IFunctionDefinition> FunDefP =
			from _0 in TokenP ("public ")
			from _ in TokenP ("dynamic")
			from funName in IdentP
			from __ in TokenP ("(")
			from args in Parse.Many (
				from ___ in TokenP ("dynamic")
				from arg in VariableP
				from ____ in Parse.Optional (TokenP (","))
				select arg)
			from ___ in TokenP (")")
			from ____ in TokenP ("{")
			from mVars in Parse.Optional(
					from ______ in TokenP("dynamic")
					from variables in Parse.DelimitedBy(VariableP, TokenP(","))
                    from _______ in TokenP(";")
				    select variables.ToList())
				from statements in Parse.Many(StatementP)
				from _____ in TokenP ("}")
			select new FunctionDefinition {
				name = funName,
				arguments = args.ToList(),
				variables = mVars.IsEmpty ? new List<IVariable>() : mVars.Get(),
				statements = statements.ToList()
			};

		static Parser<INamespace> NamespaceP =
			from _ in TokenP("public")
				from __ in TokenP("class")
				from namespaceName in IdentP
				from ___ in TokenP("{")
				from funs in Parse.Many(FunDefP)
				from ____ in TokenP("}")
				select new Namespace {
					namespaceName = namespaceName,
					functions = funs.ToList()
				};

	}
}