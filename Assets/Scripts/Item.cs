using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour {

	GameObject _attachment;

	Rigidbody _rigidbody;
	Collider _collider;
	

	public GameObject owner { get; private set; }

	public bool held { get { return (owner != null); } }

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

	public void PickUp(GameObject owner, GameObject attachment) {
		if (held)
			throw new InvalidOperationException(string.Format(
				"{0} is already being held by {1}", gameObject, owner));

		this.owner = owner;
		_attachment = attachment;

		transform.parent = _attachment.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		enableCollision = false;
		enablePhysics = false;
	}

	public void Drop(Vector3 force = new Vector3()) {
		if (!held)
			throw new InvalidOperationException(string.Format(
				"{0} is not held by anything", gameObject));

		owner = null;
		_attachment = null;

		transform.parent = null;

		enableCollision = true;
		enablePhysics = true;

		_rigidbody.AddForce(force);
	}

}
