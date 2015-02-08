using System;
using System.Linq;
using System.Collections.Generic;

namespace VisualPracticalLanguage
{
	public static class SugarExtension
	{
		public static B With<T, B>(this T obj, Func<T, B> func, B defaultObj)
		{
			if (obj != null) {
				return func (obj);
			}
			return defaultObj;
		}

		public static T With<T>(this T obj, Action<T> action)
		{
			if (obj != null) {
				action (obj);
			}
			return obj;
		}



		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> list)
		{
			if (list == null) {
				return new List<T>();
			}

			return list;
		}
		
		public static bool Empty<T>(this IEnumerable<T> list)
		{
			return list == null || !list.Any ();
		}

		public static void IterSep<T>(this IEnumerable<T> list, Action<T> action, Action<T> separatorAction)
		{
			var nnlist = list.EmptyIfNull ();
			if (nnlist.Any ()) {
				action (nnlist.First());
			}

			var restlist = nnlist.Skip (1);
			if (restlist.Any ()) {
				separatorAction (nnlist.First ());
				IterSep (restlist, action, separatorAction);
			}
		}
	}
}

