
public static class HashHelper {

	public static int GetHashCode(params object[] toHash) {
		unchecked {
			int hash = (int)2166136261;
			foreach (var obj in toHash)
				hash = hash * 16777619 ^ obj.GetHashCode();
			return hash;
		}
	}

}
