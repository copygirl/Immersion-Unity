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

		_collider.enabled = false;
		_rigidbody.isKinematic = true;
		_rigidbody.detectCollisions = false;

		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
	}

	public void Drop(Vector3 force = new Vector3()) {
		if (!held)
			throw new InvalidOperationException(string.Format(
				"{0} is not held by anything", gameObject));

		owner = null;
		_attachment = null;

		transform.parent = null;
		
		_collider.enabled = true;
		_rigidbody.isKinematic = false;
		_rigidbody.detectCollisions = true;
		_rigidbody.AddForce(force);
	}

}
