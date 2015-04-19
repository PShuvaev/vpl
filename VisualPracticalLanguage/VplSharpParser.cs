using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using VisualPracticalLanguage.Interface;

namespace VisualPracticalLanguage
{
    // TODO: приоритет операций в выражениях определяется исключительно скобками 
    public class VplSharpParser
    {
        private static Parser<string> TokenP(string s)
        {
            return Parse.String(s).Text().Token();
        }

        private static Parser<string> TokensP(IEnumerable<string> ss)
        {
            if (ss.Empty())
                throw new Exception("Sequence must contain at least one string!");
            var p = TokenP(ss.First());
            foreach (var np in ss.Skip(1))
                p = p.Or(TokenP(np));
            return p;
        }

        private static Parser<TC> BracketP<TC>(Parser<TC> content)
        {
            return (
                from _ in TokenP("(")
                from res in content
                from __ in TokenP(")")
                select res);
        }

        private static Parser<ICondStatement> condStatementP(string condType,
            Func<IExpression, IList<IStatement>, ICondStatement> constructor)
        {
            return
                from _ in TokenP(condType)
                from __ in TokenP("(")
                from expr in Parse.Ref(() => ExpressionP)
                from ___ in TokenP(")")
                from ____ in TokenP("{")
                from statements in Parse.Ref(() => StatementP.Many())
                from _____ in TokenP("}")
                select constructor(expr, statements.ToList());
        }

        private static readonly Parser<string> IdentP =
            Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Text().Token();

        private static readonly Parser<string> QuotedTextP =
            (from _ in Parse.Char('"')
                from txt in Parse.CharExcept('"').Many().Text()
                from __ in Parse.Char('"')
                select txt).Token();

        public static Parser<IFunctionCall> FunCallP = (
            from funName in (
                from library in IdentP
                from _ in Parse.Char('.')
                from fname in IdentP
                select library + '.' + fname
                ).Or(IdentP)
            from _ in TokenP("(")
            from arguments in (
                from __ in TokenP(")")
                select new List<IExpression>()
                ).Or(
                    from args in Parse.Ref(() => ExpressionP).DelimitedBy(TokenP(","))
                    from __ in TokenP(")")
                    select args.ToList()
                )
            select new FunctionCall
            {
                arguments = arguments,
                function = new FunctionDeclaration
                {
                    name = funName,
                    isBinOperation = false
                }
            });

        private static readonly Parser<IVariable> VariableP =
            from varName in IdentP
            select new Variable {varName = varName};

        private static readonly Parser<IVariableRef> VariableRefP =
            from varName in IdentP
            select new VariableRef {varName = varName};

        public static Parser<IConstExpression> ConstP =
            (from _ in TokenP("null")
                select new ConstExpression {constValue = null})
                .Or(from str in QuotedTextP
                    select new ConstExpression {constValue = str})
                .Or(from dec in Parse.Decimal.Token()
                    select new ConstExpression {constValue = decimal.Parse(dec)});

        public static Parser<IExpression> ExpressionP =
            from firstExpr in
                BracketP(Parse.Ref(() => ExpressionP))
                    .Or(Parse.Ref(() => ConstP))
                    .Or(Parse.Ref(() => FunCallP))
                    .Or(Parse.Ref(() => VariableRefP))
            from operationPart in (from ____ in Parse.WhiteSpace.Many()
                from op in TokensP(new[] {"+", "-", "*", "/", ">", "<", "==", "!=", ">=", "<="})
                from v2 in Parse.Ref(() => ExpressionP)
                select new FunctionCall
                {
                    arguments = new List<IExpression> {v2},
                    function = new FunctionDeclaration
                    {
                        isBinOperation = true,
                        name = op + ""
                    }
                }).Optional()
            select (
                operationPart.IsEmpty
                    ? firstExpr
                    : operationPart.Get().With(fun => { fun.arguments.Insert(0, firstExpr); }));

        private static readonly Parser<IReturnStatement> ReturnStatementP =
            from _ in TokenP("return")
            from mExpr in Parse.Ref(() => ExpressionP).Optional()
            from __ in TokenP(";")
            select new ReturnStatement
            {
                expression = mExpr.IsEmpty ? null : mExpr.Get()
            };

        private static readonly Parser<ISetVariableStatement> SetStatementP =
            from variable in VariableRefP
            from _ in TokenP("=")
            from expr in Parse.Ref(() => ExpressionP)
            from __ in TokenP(";")
            select new SetVariableStatement
            {
                variableRef = variable,
                expression = expr
            };

        private static readonly Parser<IFunCallStatement> CallProcStatementP =
            from fcall in Parse.Ref(() => FunCallP)
            from _ in TokenP(";")
            select new FunCallStatement
            {
                functionCall = fcall
            };

        private static readonly Parser<IStatement> StatementP =
            condStatementP("if", (expr, stmts) => new IfStatement {condition = expr, statements = stmts})
                .Or(condStatementP("while", (expr, stmts) => new WhileStatement {condition = expr, statements = stmts}))
                .Or(SetStatementP.Select(x => (IStatement) x))
                // Внимание! ReturnStatementP должен быть объявлен ранее CallProcStatementP, ибо return(1) воспримется как вызов функции
                .Or(ReturnStatementP)
                .Or(CallProcStatementP.Select(x => (IStatement) x));

        public static Parser<IFunctionDefinition> FunDefP =
            from _0 in TokenP("public ")
            from _ in TokenP("dynamic")
            from funName in IdentP
            from __ in TokenP("(")
            from args in (from ___ in TokenP("dynamic")
                from arg in VariableP
                from ____ in TokenP(",").Optional()
                select arg).Many()
            from ___ in TokenP(")")
            from ____ in TokenP("{")
            from mVars in (from ______ in TokenP("dynamic")
                from variables in VariableP.DelimitedBy(TokenP(","))
                from _______ in TokenP(";")
                select variables.ToList()).Optional()
            from statements in StatementP.Many()
            from _____ in TokenP("}")
            select new FunctionDefinition
            {
                name = funName,
                arguments = args.ToList(),
                variables = mVars.IsEmpty ? new List<IVariable>() : mVars.Get(),
                statements = statements.ToList()
            };

        public static Parser<INamespace> NamespaceP =
            from usingNamespaces in (from _ in TokenP("using")
                from name in IdentP
                from __ in TokenP(";")
                select name).Many()
            from _ in TokenP("public")
            from __ in TokenP("class")
            from namespaceName in IdentP
            from ___ in TokenP("{")
            from funs in FunDefP.Many()
            from ____ in TokenP("}")
            select new Namespace
            {
                namespaceName = namespaceName,
                importedDlls = usingNamespaces.ToList(),
                functions = funs.ToList()
            };
    }
}