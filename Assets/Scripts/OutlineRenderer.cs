//
// Script that applies outline to an object when added.
// Source: http://nihilistdev.blogspot.de/2013/05/outline-in-unity-with-mesh-transparency.html
//

using System.Linq;
using UnityEngine;

public class OutlineRenderer : MonoBehaviour {

	GameObject[] _outlineObjects;

	void Start() {
		_outlineObjects = GetComponentsInChildren<MeshFilter>()
			.Select(meshFilter => {
				var obj = new GameObject("Outline");
				
				obj.transform.parent = meshFilter.transform;
				obj.transform.ResetPositionAndRotation();
				obj.transform.localScale = Vector3.one;

				obj.AddComponent<MeshFilter>().mesh = meshFilter.sharedMesh;
				obj.AddComponent<MeshRenderer>().material =
					new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
				
				return obj;
			}).ToArray();
	}

	void OnDestroy() {
		foreach (var obj in _outlineObjects)
			Destroy(obj);
		_outlineObjects = null;
	}

}
