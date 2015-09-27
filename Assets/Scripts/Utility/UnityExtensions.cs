using UnityEngine;

public static class UnityExtensions {

	/// <summary> Resets the transform's position and rotation. </summary>
	public static void ResetPositionAndRotation(this Transform transform) {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

}
