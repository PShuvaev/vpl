using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VplLibrary
{
	public static class Кортеж
	{
		public static void Сообщение(object obj){
			MessageBox.Show(obj.ToString());
		}

        public static object Кортеж2(object элемент1, object элемент2) {
            return new Dictionary<object, object> { { 1, элемент1 }, { 2, элемент2 } };
        }
        public static object Кортеж3(object элемент1, object элемент2, object элемент3) {
            return new Dictionary<object, object> { { 1, элемент1 }, { 2, элемент2 }, { 3, элемент3 } };
        }
        public static object Кортеж4(object элемент1, object элемент2, object элемент3, object элемент4) {
            return new Dictionary<object, object> { { 1, элемент1 }, { 2, элемент2 }, { 3, элемент3 }, { 4, элемент4 } };
        }

        public static object ЭлементНаПозиции(object кортеж, object позиция)
        {
            var dict = (IDictionary<object, object>)кортеж;
            var pos = Convert.ToDecimal(позиция);
            object val;
            dict.TryGetValue(decimal.ToInt32(pos), out val);
            return val;
        }
        public static object УстановитьЭлементНаПозиции(object кортеж, object позиция, object элемент)
        {
            var dict = (IDictionary<object, object>)кортеж;
            var pos = Convert.ToDecimal(позиция);
            dict[decimal.ToInt32(pos)] = элемент;
            return null;
        }
    }
}

