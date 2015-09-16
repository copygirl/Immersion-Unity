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
		rightHand = _equipment.AddSlot(rightHandAttachment, "hand", "held", "right");
		leftHand  = _equipment.AddSlot(leftHandAttachment, "hand", "held", "left");
		_camera = Camera.main.GetComponent<CameraController>();
	}
	
	void Update() {
		Item highlightedItem = null;

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

		HandleHand(rightHand, "Right Hand", highlightedItem);
		HandleHand(leftHand, "Left Hand", highlightedItem);
	}


	void HandleHand(EquipmentSlot slot, string input, Item highlightedItem) {
		var down = Input.GetButton(input);
		var pressed = Input.GetButtonDown(input);
		var released = Input.GetButtonUp(input);

		var drop = Input.GetButton("Drop");
		
		if (slot.occupied) {
			if (drop && pressed) {
				var item = slot.item;
				slot.Unequip();
				item.OnDrop();
				item.GetComponent<Rigidbody>().AddForce((Vector3.up + _camera.transform.forward) * 200.0f);
			}
		} else if ((highlightedItem != null) && pressed) {
			highlightedItem.OnPickup();
			slot.Equip(highlightedItem);
		}
	}

}
