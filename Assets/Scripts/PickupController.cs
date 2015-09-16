using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class PickupController : MonoBehaviour {

	public float pickupRange = 2.0f;

	public GameObject rightHandAttachment;
	public GameObject leftHandAttachment;

	public EquipmentSlot rightHand { get; private set; }
	public EquipmentSlot leftHand { get; private set; }
	
	Equipment _equipment;
	CameraController _camera;

	void Start() {
		_equipment = GetComponent<Equipment>();
		rightHand = _equipment.AddSlot(rightHandAttachment, EquipmentRegion.HAND, EquipmentTag.HELD, EquipmentTag.RIGHT);
		leftHand  = _equipment.AddSlot(leftHandAttachment, EquipmentRegion.HAND, EquipmentTag.HELD, EquipmentTag.LEFT);
		_camera = Camera.main.GetComponent<CameraController>();
	}
	
	void Update() {
		var drop = Input.GetButton("Drop");

		var right = Input.GetButton("Right Hand");
		var rightPressed = Input.GetButtonDown("Right Hand");
		var rightReleased = Input.GetButtonUp("Right Hand");

		Item highlightedItem = null;

		if (rightHand.occupied) {
			if (drop && rightPressed) {
				var item = rightHand.item;
				rightHand.Unequip();
				item.OnDrop();
				item.GetComponent<Rigidbody>().AddForce((Vector3.up + _camera.transform.forward) * 200.0f);
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

		if (!rightHand.occupied && (highlightedItem != null) && rightPressed) {
			highlightedItem.OnPickup();
			rightHand.Equip(highlightedItem);
		}
	}

}
