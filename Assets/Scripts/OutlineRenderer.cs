//
// Script that applies outline to an object when added.
// Source: http://nihilistdev.blogspot.de/2013/05/outline-in-unity-with-mesh-transparency.html
//

using UnityEngine;
using System.Collections;

public class OutlineRenderer : MonoBehaviour {

	GameObject _outlineObj;

	void Start() {
		_outlineObj = new GameObject("Outline");

		_outlineObj.transform.parent = transform;
		_outlineObj.transform.localPosition = Vector3.zero;
		_outlineObj.transform.localRotation = Quaternion.identity;
		_outlineObj.transform.localScale = Vector3.one;

		_outlineObj.AddComponent<MeshFilter>().mesh =
			GetComponent<MeshFilter>().sharedMesh;
		_outlineObj.AddComponent<MeshRenderer>().material =
			new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
	}

	void OnDestroy() {
		Destroy(_outlineObj);
		_outlineObj = null;
	}

}
