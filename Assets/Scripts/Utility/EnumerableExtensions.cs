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

}
