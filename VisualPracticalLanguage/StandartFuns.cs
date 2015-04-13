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
			{"равно", () => MakeBinaryOp("==")},
			{">", () => MakeBinaryOp(">")},
			{"<", () => MakeBinaryOp("<")},
			{"не равно", () => MakeBinaryOp("!=")},
			{"присвоить", () => new VSetVariable()},
			{"если", () => new VIfStatement()},
			{"пока", () => new VWhileStatement()},
			{"вернуть ", () => new VReturnStatement()},
			{"число", () => {
					var val = DiverseUtilExtensions.ShowDialog("Число", "Введите значение");
					decimal x;
					return decimal.TryParse(val, out x) ? new VNumberConst(x) : null;
				}},
			{"строка", () => {
					var val = DiverseUtilExtensions.ShowDialog("Строка", "Введите значение");
					return new VStringConst(val);
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

