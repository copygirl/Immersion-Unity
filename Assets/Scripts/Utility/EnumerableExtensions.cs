using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumerableExtensions {

	/// <summary> Returns an enumerable that, when iterated, returns the
	///           provided elements before the source enumerable. </summary>
	public static IEnumerable<T> Precede<T>(this IEnumerable<T> enumerable, params T[] elements) {
		return elements.Concat(enumerable);
	}

	/// <summary> Returns an enumerable that, when iterated, returns the
	///           provided elements after the source enumerable. </summary>
	public static IEnumerable<T> Follow<T>(this IEnumerable<T> enumerable, params T[] elements) {
		return enumerable.Concat(elements);
	}


	/// <summary> Returns if the source enumerable contains all elements in the specified enumerable. </summary>
	public static bool Contains<T>(this IEnumerable<T> enumerable, IEnumerable<T> elements) {
		foreach (var element in elements)
			if (!Enumerable.Contains(enumerable, element))
				return false;
		return true;
	}
	/// <summary> Returns if the enumerable contains all the specified elements. </summary>
	public static bool Contains<T>(this IEnumerable<T> enumerable, params T[] elements) {
		if (elements.Length <= 0)
			throw new ArgumentException("elements is empty", "elements");
		return enumerable.Contains((IEnumerable<T>)elements);
	}

	/// <summary> Returns a string of the elements in this enumerable
	///           joined together, seperated by the specified seperator.
	///           Returns an empty string if the enumerable is empty. </summary>
	public static string Join<T>(this IEnumerable<T> enumerable, string seperator) {
		var enumerator = enumerable.GetEnumerator();
		if (!enumerator.MoveNext()) return "";

		var builder = new StringBuilder();
		var last = enumerator.Current;

		while (enumerator.MoveNext()) {
			builder.Append(last).Append(seperator);
			last = enumerator.Current;
		}
		builder.Append(last);

		return builder.ToString();
	}

}
