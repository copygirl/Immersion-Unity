using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float walkSpeed = 2.0f;
	public float sprintMultiplier = 1.75f;
	public float jumpVelocity = 8.0f;

	public float minTurnRotation = 40.0f;

	public float gravity = 10.0f;

	Vector3 movement = Vector3.zero;
	bool isJumping = false;
	bool isSprinting = false;

	CharacterController _controller;
	CameraController _camera;

	void Start() {
		_controller = GetComponent<CharacterController>();
		_camera = Camera.main.GetComponent<CameraController>();
	}
	
	void Update() {
		UpdateMovement();
		UpdateLooking();
	}

	void UpdateMovement() {
		var forward = Input.GetAxisRaw("Vertical");
		var sideways = Input.GetAxisRaw("Horizontal");
		var sprinting = Input.GetButton("Sprint");
		var jump = Input.GetButtonDown("Jump");
		
		var lookRotation = Quaternion.Euler(0.0f, _camera.transform.eulerAngles.y, 0.0f);

		movement.y -= gravity * Time.deltaTime;

		isSprinting = false;

		if (_controller.isGrounded) {
			isJumping = false;
			
			var vector = new Vector3(sideways, 0.0f, forward);
			if (vector.magnitude > 1.0f) vector.Normalize();
			
			var speedMultiplier = walkSpeed;
			if ((vector.z > 0.5f) && (sprinting)) {
				isSprinting = true;
				speedMultiplier *= sprintMultiplier;
			}

			movement = lookRotation * vector * speedMultiplier;

			if (jump) movement.y += jumpVelocity;
		}

		var previousCameraRotation = _camera.rotation;
		var directionVector = new Vector3(sideways, 0.0f, 1.0f + Mathf.Abs(forward) * 2);
		var intendedBodyLookAt = lookRotation * directionVector;
		intendedBodyLookAt = Vector3.Lerp(transform.forward, intendedBodyLookAt,
		                                  Mathf.Max(0.0f, Mathf.Min(1.0f, movement.magnitude - 0.2f)) * Time.deltaTime * 4);
		transform.LookAt(transform.position + intendedBodyLookAt);
		_camera.rotation = previousCameraRotation;

		
		_controller.Move(movement * Time.deltaTime);
	}
	
	void UpdateLooking() {
		var turn = 0.0f;
		var lookDelta = Mathf.DeltaAngle(transform.eulerAngles.y, _camera.transform.eulerAngles.y);
		if (lookDelta < -minTurnRotation) turn = lookDelta + minTurnRotation;
		else if (lookDelta > minTurnRotation) turn = lookDelta - minTurnRotation;
		
		Turn(turn * Time.deltaTime * 20.0f);
	}

	void Turn(float angle)
	{
		var turnVector = new Vector3(0.0f, angle, 0.0f);
		transform.eulerAngles += turnVector;
		_camera.eulerAngles -= turnVector;
	}

}
