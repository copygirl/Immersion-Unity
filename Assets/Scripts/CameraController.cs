using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Vector2 sensitivity = new Vector2(6.0f, 6.0f);

	public float minLookAngle = -80.0f;
	public float maxLookAngle = 80.0f;


	Transform _parentTransform;

	public Vector3 position { get { return transform.position; } }

	public Quaternion rotation {
		get { return _parentTransform.rotation; }
		set { _parentTransform.rotation = value; }
	}
	public Vector3 eulerAngles {
		get { return _parentTransform.eulerAngles; }
		set { _parentTransform.eulerAngles = value; }
	}

	public float pitch {
		get { return eulerAngles.x; }
		set { eulerAngles = new Vector3(value, yaw, roll); }
	}
	public float yaw {
		get { return eulerAngles.y; }
		set { eulerAngles = new Vector3(pitch, value, roll); }
	}
	public float roll {
		get { return eulerAngles.z; }
		set { eulerAngles = new Vector3(yaw, pitch, value); }
	}


	void Start() {
		_parentTransform = transform.parent;
	}

	void Update() {
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		if (Cursor.lockState == CursorLockMode.None) return;
			
		var mouseHorizontal = Input.GetAxis("Mouse X");
		var mouseVertical = Input.GetAxis("Mouse Y");

		pitch = Mathf.Max(minLookAngle, Mathf.Min(maxLookAngle,
			Mathf.DeltaAngle(0.0f, pitch - mouseVertical * sensitivity.y)));

		yaw += mouseHorizontal * sensitivity.x;
	}

}
