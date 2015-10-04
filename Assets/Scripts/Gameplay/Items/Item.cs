using System.Linq;
using UnityEngine;

/// <summary> Component identifying game objects that function as items
///           and may be picked up, moved or manipulated as such. </summary>
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, ISerializationCallbackReceiver {

	Rigidbody _rigidbody;
	Collider[] _colliders;

	#region Public properties

	/// <summary> Gets the equipment slot the item is currently carried in, null if none. </summary>
	// TODO: Add support for items which are equipped in multiple slots.
	public EquipmentSlot slot { get; private set; }

	/// <summary> Gets or sets whether the item should be highlighted for this frame. </summary>
	public bool highlighted { get; set; }


	/// <summary> Gets if the item is currently equipped somewhere / not on the ground. </summary>
	public bool equipped { get { return (slot != null); } }

	/// <summary> Gets or sets the mass of the item in kilograms. </summary>
	public virtual float weight {
		get { return _rigidbody.mass; }
		set { _rigidbody.mass = value; }
	}


	/// <summary> Gets or sets whether other colliders can collide with this item. </summary>
	public bool enableCollision {
		get { return _colliders[0].isTrigger; }
		set { foreach (var col in _colliders) col.isTrigger = !value; }
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

	#region MonoBehavior methods

	void Start() {
		_rigidbody = GetComponent<Rigidbody>();
		_colliders = GetComponentsInChildren<Collider>();
	}

	void LateUpdate() {
		var outline = GetComponent<OutlineRenderer>();

		if (outline != null) {
			if (!highlighted) Destroy(outline);
		} else if (highlighted)
			gameObject.AddComponent<OutlineRenderer>();

		highlighted = false;
	}

	#endregion

	#region Public methods

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

	#endregion

	#region ISerializationCallbackReceiver implementation

	public void OnBeforeSerialize() {  }

	public void OnAfterDeserialize() {
		var equipment = GetComponentInParent<Equipment>();
		if (equipment == null) return;
		slot = equipment.FirstOrDefault(s => (s.attachment == transform.parent.gameObject));
	}

	#endregion

}
