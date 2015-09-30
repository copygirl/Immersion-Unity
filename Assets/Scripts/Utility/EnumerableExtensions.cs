using System;
using System.Collections.Generic;
using System.Linq;

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
	
}
