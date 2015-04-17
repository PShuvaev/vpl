using System;
using System.Windows.Forms;

namespace VplLibrary
{
	public static class VplLibrary1
	{
        public static object GlobalVar;

        public static dynamic УстановитьГлобальнуюПеременную(object o)
        {
            GlobalVar = o;
            return null;
        }

		public static void Сообщение(object obj){
			MessageBox.Show(obj.ToString());
		}

        public static Tuple2 Кортеж(object o1, object o2)
        {
            return new Tuple2(o1, o2);
        }

        public static object Элемент1(Tuple2 t)
        {
            return t.o1;
        }

        public static object Элемент2(Tuple2 t)
        {
            return t.o2;
        }
    }

    public class Tuple2
    {
        public object o1 { get; set; }
        public object o2 { get; set; }

        public Tuple2(object o1, object o2)
        {
            this.o1 = o1;
            this.o2 = o2;
        }

    }
}

