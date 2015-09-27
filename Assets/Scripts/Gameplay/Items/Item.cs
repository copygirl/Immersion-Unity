using UnityEngine;

/// <summary> Script component identifying game entities that should function as items. </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour {

	Rigidbody _rigidbody;
	Collider _collider;


	#region Public properties

	/// <summary> Gets the equipment slot the item is currently carried in, null if none. </summary>
	public EquipmentSlot slot { get; private set; }

	/// <summary> Gets or sets whether the item should be highlighted for this frame. </summary>
	public bool highlighted { get; set; }


	/// <summary> Gets or sets whether other colliders can collide with this item. </summary>
	public bool enableCollision {
		get { return _collider.enabled; }
		set { _collider.enabled = value; }
	}

	/// <summary> Gets or sets whether physics are enabled on this item.
	///           If disabled, resets the item's motion. </summary>
	public bool enablePhysics {
		get { return !_rigidbody.isKinematic; }
		set {
			_rigidbody.isKinematic = !value;
			_rigidbody.detectCollisions = value;
			// Reset motion when set to disabled.
			if (!value) {
				_rigidbody.velocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
			}
		}
	}

	#endregion


	void Start() {
		_rigidbody = GetComponent<Rigidbody>();
		_collider = GetComponent<Collider>();
	}

	void LateUpdate() {
		var outline = GetComponent<OutlineRenderer>();

		if (outline != null) {
			if (!highlighted) Destroy(outline);
		} else if (highlighted)
			gameObject.AddComponent<OutlineRenderer>();

		highlighted = false;
	}


	/// <summary> Returns if the item may be equipped in the specified slot.
	///           Doesn't check any requirement than just those of the item itself. </summary>
	public virtual bool CanEquip(EquipmentSlot slot) { return true; }

	/// <summary> Returns if the item can be unequipped from the specified slot.
	///           Doesn't check any requirement than just those of the item itself. </summary>
	public virtual bool CanUnequip(EquipmentSlot slot) { return true; }


	/// <summary> Called when the item is equipped in the specified slot. </summary>
	public virtual void OnEquip(EquipmentSlot slot) {
		this.slot = slot;

		transform.parent = slot.attachment.transform;
		transform.ResetPositionAndRotation();
	}

	/// <summary> Called when the item is unequipped from its current slot. </summary>
	public virtual void OnUnequip() {
		this.slot = null;

		transform.parent = null;
	}


	/// <summary> Called when the item is picked up from the world. </summary>
	public virtual void OnPickup() {
		enableCollision = false;
		enablePhysics = false;

	}

	/// <summary> Called when the item is dropped into the world,
	///           after being equipped or stored somewhere. </summary>
	public virtual void OnDrop() {
		enableCollision = true;
		enablePhysics = true;
	}

}
