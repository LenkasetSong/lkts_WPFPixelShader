using System;
using System.Collections.Generic;
using System.Text;

namespace WpfTPL
{
	public static class StringExtensions
	{
		internal static string Concat<T>(this string str, IEnumerable<T> enumT, string span)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var item in enumT)
			{
				sb.Append(item.ToString() + span);
			}
			str = sb.Remove(sb.Length - span.Length, span.Length).ToString();
			return str;
		}

		internal static IEnumerable<Range<T>> GetPairs<T>(this T[] array)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				yield return new Range<T>(array[i], array[i + 1]);
			}
		}
	}

	internal class Range<T>
	{
		private T t1;
		private T t2;

		public Range(T t1, T t2)
		{
			this.t1 = t1;
			this.t2 = t2;
		}
	}
}
