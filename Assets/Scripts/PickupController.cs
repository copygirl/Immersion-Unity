using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class PickupController : MonoBehaviour {
	
	Equipment _equipment;
	CameraController _camera;


	public float pickupRange = 0.5F;
	public float swapRange   = 0.25F;

	public GameObject rightHandAttachment;
	public GameObject leftHandAttachment;


	public EquipmentSlot rightHand { get; private set; }
	public EquipmentSlot leftHand { get; private set; }


	void Start() {
		_equipment = GetComponent<Equipment>();
		_camera = Camera.main.GetComponent<CameraController>();
		
		rightHand = _equipment.AddSlot(rightHandAttachment, EquipmentRegion.Hands,
		                               EquipmentTag.Held, EquipmentTag.Right);
		leftHand  = _equipment.AddSlot(leftHandAttachment, EquipmentRegion.Hands,
		                               EquipmentTag.Held, EquipmentTag.Left);
	}
	
	void Update() {
		var right    = Input.GetButton("Right Hand");
		var left     = Input.GetButton("Left Hand");
		var interact = Input.GetButtonDown("Interact");
		
		if (right && left) {
			var rightItem = rightHand.item;
			var leftItem  = leftHand.item;
			// Item in either or both hands: Allow for swapping
			// them, if the hands are held together close enough.
			// TODO: Add delay and "animation" of item switching hands.
			if (rightHand.occupied || leftHand.occupied) {
				var inRange = (Vector3.Distance(rightHandAttachment.transform.position,
				                                leftHandAttachment.transform.position) <= swapRange);
				if (interact && inRange && (rightItem != leftItem) &&
					(!rightHand.occupied || (rightItem.CanUnequip(rightHand) && rightItem.CanEquip( leftHand))) &&
				    (! leftHand.occupied || ( leftItem.CanUnequip( leftHand) &&  leftItem.CanEquip(rightHand)))) {
					if (rightItem != null) rightHand.Unequip();
					if ( leftItem != null)  leftHand.Unequip();
					if (rightItem != null)  leftHand.Equip(rightItem);
					if ( leftItem != null) rightHand.Equip(leftItem);
				}
				if (inRange) {
					if (rightItem != null) rightItem.highlighted = true;
					if ( leftItem != null)  leftItem.highlighted = true;
				}
			// TODO: This is where you'd pick up items with both hands.
			} else {  }
		} else {
			HandleHand(rightHand, right, interact);
			HandleHand(leftHand, left, interact);
		}
	}


	void HandleHand(EquipmentSlot slot, bool down, bool interact) {
		if (slot.occupied) {
			var item = slot.item;
			if (down && interact && item.CanUnequip(slot)) {
				slot.Unequip();
				item.OnDrop();
				item.GetComponent<Rigidbody>().AddForce(
					(Vector3.up + _camera.transform.forward) * 200.0f);
			}
		} else if (down) {
			Item highlightedItem = null;

			var pos = slot.attachment.transform.position;
			var distance = pickupRange;
			var colliders = Physics.OverlapSphere(pos, pickupRange);

			foreach (var col in colliders) {
				var item = col.GetComponentInParent<Item>();
				if ((item == null) || item.equipped ||
				    !item.CanEquip(slot)) continue;

				var dis = Vector3.Distance(pos, col.ClosestPointOnBounds(pos));
				if (dis <= distance) {
					highlightedItem = item;
					distance = dis;
				}
			}

			if (highlightedItem != null) {
				highlightedItem.highlighted = true;

				if (interact) {
					highlightedItem.OnPickup();
					slot.Equip(highlightedItem);
				}
			}
		}
	}

}
