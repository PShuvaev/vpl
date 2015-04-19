using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VisualPracticalLanguage
{
    public static class SugarExtension
    {
        public static B OrDef<T, B>(this T obj, Func<T, B> func, B defaultObj = default(B))
            where T : class
        {
            if (obj != null)
            {
                return func(obj);
            }
            return defaultObj;
        }

        public static T With<T>(this T obj, Action<T> action)
            where T : class
        {
            if (obj != null)
            {
                action(obj);
            }
            return obj;
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> list)
        {
            return list ?? Enumerable.Empty<T>();
        }

        public static bool Empty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static void DoAll(this IEnumerable list)
        {
            foreach (var _ in list)
            {
            }
        }

        public static void IterSep<T>(this IEnumerable<T> list, Action<T> action, Action<T> separatorAction)
        {
            var nnlist = list.EmptyIfNull();
            if (nnlist.Any())
            {
                action(nnlist.First());
            }

            var restlist = nnlist.Skip(1);
            if (restlist.Any())
            {
                separatorAction(nnlist.First());
                IterSep(restlist, action, separatorAction);
            }
        }

        public static IList<T> Intercalate<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var num1 = list1.GetEnumerator();
            var num2 = list2.GetEnumerator();
            var list = new List<T>();

            while (num1.MoveNext())
            {
                list.Add(num1.Current);
                if (num2.MoveNext())
                    list.Add(num2.Current);
            }

            return list;
        }
    }
}