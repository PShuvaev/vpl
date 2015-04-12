using System;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public static class StandartFuns
	{
		public static Dictionary<string, Func<DraggableControl>> Funs = new Dictionary<string, Func<DraggableControl>>(){
			{"+", () => MakeBinaryOp("+")},
			{"-", () => MakeBinaryOp("-")},
			{"/", () => MakeBinaryOp("/")},
			{"*", () => MakeBinaryOp("*")},
			{"=", () => MakeBinaryOp("==")},
			{">", () => MakeBinaryOp(">")},
			{"<", () => MakeBinaryOp("<")},
			{"!=", () => MakeBinaryOp("!=")},
			{"если", () => new VIfStatement()},
			{"пока", () => new VWhileStatement()},
			{"вернуть ", () => new VReturnStatement()},
			{"константа", () => {
					var val = DiverseUtilExtensions.ShowDialog("Новая константа", "Введите значение");
					if(val.StartsWith("\"")) return new VStringConst(val);

					decimal x;
					if(decimal.TryParse(val, out x)) return new VNumberConst(x);

					return null;
				}},
			{"функция", () => {
					// TODO: если пользователь ничего не вводит, возвращать null
					var name = DiverseUtilExtensions.ShowDialog("Введите имя функции", "Новая функция");
					return new VFunction(name);
				}}
		};

		private static VBinaryOp MakeBinaryOp(string symbol){
			return new VBinaryOp (new FunctionDeclaration { name = symbol, isBinOperation = true });
		}
	}
}

