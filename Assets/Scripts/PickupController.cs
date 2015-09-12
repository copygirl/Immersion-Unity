using UnityEngine;

public class PickupController : MonoBehaviour {

	public float pickupRange = 2.0f;

	public GameObject rightHand;
	public GameObject leftHand;

	public Item rightHandItem { get; set; }
	public Item leftHandItem { get; set; }
	
	CameraController _camera;


	void Start() {
		_camera = Camera.main.GetComponent<CameraController>();
	}
	
	void Update() {
		var drop = Input.GetButton("Drop");
		var right = Input.GetButton("Right Hand");
		var rightPressed = Input.GetButtonDown("Right Hand");
		var rightReleased = Input.GetButtonUp("Right Hand");

		Item highlightedItem = null;

		if (rightHandItem != null) {
			if (drop && rightPressed) {
				rightHandItem.Drop((Vector3.up + _camera.transform.forward) * 200.0f);
				rightHandItem = null;
			}
		} else {
			RaycastHit hitInfo;
			if (Physics.Raycast(_camera.position, _camera.transform.forward, out hitInfo, pickupRange)) {
				var hitObject = hitInfo.collider.gameObject;
				if (hitObject != null) {
					var item = hitObject.GetComponent<Item>();
					if (item != null) {
						item.highlighted = true;
						highlightedItem = item;
					}
				}
			}
		}

		if ((rightHandItem == null) && (highlightedItem != null) && rightPressed) {
			highlightedItem.PickUp(gameObject, rightHand);
			rightHandItem = highlightedItem;
		}
	}

}
