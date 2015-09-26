using System.Collections.Generic;

public static class EnumerableExtensions {

	/// <summary> Returns an enumerable that, when iterated, returns the
	///           provided elements before the source enumerable. </summary>
	public static IEnumerable<T> Precede<T>(this IEnumerable<T> enumerable, params T[] elements) {
		foreach (var element in elements)
			yield return element;
		foreach (var element in enumerable)
			yield return element;
	}

	/// <summary> Returns an enumerable that, when iterated, returns the
	///           provided elements after the source enumerable. </summary>
	public static IEnumerable<T> Follow<T>(this IEnumerable<T> enumerable, params T[] elements) {
		foreach (var element in enumerable)
			yield return element;
		foreach (var element in elements)
			yield return element;
	}

}
