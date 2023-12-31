﻿using UnityEngine;
using UnityEngine.Serialization;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public PauseScript pause;
	public AudioSource walk;
	// public bool walk;
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 1.0f;                   // Default run speed.
	public float sprintSpeed = 2.0f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
	public string jumpButton = "Jump";              // Default jump button.
	public float jumpHeight = 1.5f;                 // Default jump height.
	public float jumpInertialForce = 10f;          // Default horizontal inertial force when jumping.
	public float flapSpeed = 0f;

	private float speed, speedSeeker;               // Moving speed.
	private int jumpBool;                           // Animator variable related to jumping.
	private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
	private bool jump;                              // Boolean to determine whether or not the player started a jump.
	private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
	public float horizontal;
	public float vertical;
	private Animator animator;

	// Start is always called after any Awake functions.
	void Start()
	{
		animator = GetComponent<Animator>();
		// Set up the references.
		if (!pause.paused) Cursor.lockState = CursorLockMode.Locked;
		else if (pause.paused) Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 1f;
		jumpBool = Animator.StringToHash("Jump");
		groundedBool = Animator.StringToHash("Grounded");
		behaviourManager.GetAnim.SetBool(groundedBool, true);

		// Subscribe and register this behaviour as the default behaviour.
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
		speedSeeker = runSpeed;
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		// if (jump && !behaviourManager.GetAnim.GetBool(jumpBool))
		// {
		// 	// Set jump related parameters.
		// 	// behaviourManager.LockTempBehaviour(this.behaviourCode);
		// 	behaviourManager.GetAnim.SetBool(jumpBool, true);
		// 	// Is a locomotion jump?
		// 	// Temporarily change player friction to pass through obstacles.
		// 	GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
		// 	GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		// 	// Remove vertical velocity to avoid "super jumps" on slope ends.
		// 	RemoveVerticalVelocity();
		// 	// Set jump vertical impulse velocity.
		// 	float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
		// 	velocity = Mathf.Sqrt(velocity);
		// 	behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
		// }
		
		if (!pause.paused && !pause.health.playerDead) Cursor.lockState = CursorLockMode.Locked;
		else if (pause.paused || pause.health.playerDead) Cursor.lockState = CursorLockMode.None;
		
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		if (behaviourManager.IsGrounded()) 
		{
			if ((horizontal == 0 && vertical == 0)) walk.Play();
		}
		else walk.Pause();

		// Get jump input.
		if (!jump && Input.GetButtonDown(jumpButton))
		{
			jump = true;
		}

		behaviourManager.GetCamScript.Aim();
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Call the basic movement manager.
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

		// Call the jump manager.
		JumpManagement();

		AirControl();
		
	}
	
	void JumpManagement()
	{
		// Start a new jump.
		if (jump && !behaviourManager.GetAnim.GetBool(jumpBool))
		{
			// Set jump related parameters.
			behaviourManager.LockTempBehaviour(this.behaviourCode);
			behaviourManager.GetAnim.SetBool(jumpBool, true);
			// Is a locomotion jump?
			// Temporarily change player friction to pass through obstacles.
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
			// Remove vertical velocity to avoid "super jumps" on slope ends.
			RemoveVerticalVelocity();
			// Set jump vertical impulse velocity.
			float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
			velocity = Mathf.Sqrt(velocity);
			behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
			
		}
		// Is already jumping?
		else if (behaviourManager.GetAnim.GetBool(jumpBool))
		{
			// Has landed?
			if (behaviourManager.IsGrounded())
			{
				// Change back player friction to default.
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
				behaviourManager.UnlockTempBehaviour(this.behaviourCode);
				// Set jump related parameters.
			}
			if ((behaviourManager.GetRigidBody.velocity.y > flapSpeed))
			{
				behaviourManager.GetAnim.SetBool(groundedBool, true);
				jump = false;
				behaviourManager.GetAnim.SetBool(jumpBool, false);
			}
		}
	}
	

	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical)
	{
		// On ground, obey gravity.
		// if (behaviourManager.IsGrounded())
		behaviourManager.GetRigidBody.useGravity = true;

		// Avoid takeoff when reached a slope end.
		// if (!behaviourManager.GetAnim.GetBool(jumpBool) && behaviourManager.GetRigidBody.velocity.y > 0)
		// {
		// 	RemoveVerticalVelocity();
		// }

		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		// Set proper speed.
		Vector2 dir = new Vector2(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// This is for PC only, gamepads control speed via analog stick.
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = sprintSpeed;
		}

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
	}

	void AirControl()
	{
		if ((horizontal != 0 || vertical != 0) && !isColliding)
		{
			behaviourManager.GetRigidBody.AddForce(transform.forward * (jumpInertialForce * Physics.gravity.magnitude * sprintSpeed), ForceMode.Acceleration);
		}
	}

	// Remove vertical rigidbody velocity.
	private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
		horizontalVelocity.y = 0;
		behaviourManager.GetRigidBody.velocity = horizontalVelocity;
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		isColliding = true;
		// Slide on vertical obstacles
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
}
