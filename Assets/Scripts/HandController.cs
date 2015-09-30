using System;
using UnityEngine;

public class HandController : MonoBehaviour {

	Vector3 _originPos;
	Vector3 _originRot;

	Vector2 _stance = Vector2.zero;
	bool _inversed = false;
	bool _locked = false;
	bool _lockedPressed = false;

	public GameObject hand;
	public string input;


	void Start() {
		_originPos = hand.transform.localPosition;
		_originRot = hand.transform.localEulerAngles;
	}

	void Update() {
		var down = Input.GetButton(input);
		var pressed = Input.GetButtonDown(input);

		if (pressed) {
			_inversed = Input.GetKey(KeyCode.F);
			_locked = false;
		}

		if (down) {
			if (Input.GetKeyDown(KeyCode.R))
				_lockedPressed = _locked = !_locked;
			if (!_lockedPressed) {
				var x = Input.GetAxis("Mouse X") / 10;
				var y = Input.GetAxis("Mouse Y") / 10;

				if (_inversed) {
					x *= -1;
					y *= -1;
				}

				_stance = new Vector2(Mathf.Clamp(_stance.x + x, -1.0F, 1.0F),
				                      Mathf.Clamp(_stance.y + y, -1.0F, 1.0F));
			}
		} else {
			_inversed = false;
			_lockedPressed = false;
			if (!_locked)
				_stance = Vector2.Lerp(_stance, Vector2.zero, 0.2F);
		}
		
		hand.transform.localPosition = _originPos + new Vector3(
			_stance.x / 5, _stance.y / 5, _stance.x / 12 * -Math.Sign(_originPos.x));
		hand.transform.localEulerAngles = _originRot + new Vector3(
			_stance.y * -60, 0.0F, _stance.x * -60);
	}

}
