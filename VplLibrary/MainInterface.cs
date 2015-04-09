using System;
using System.Windows.Forms;

namespace VplLibrary
{
	public class MainInterface
	{
		public static void Сообщение(object obj){
			MessageBox.Show(obj.ToString());
		}

		public static void Main(){
			Сообщение ("Привет!");
		}
	}
}

