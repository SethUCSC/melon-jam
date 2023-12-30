using UnityEngine;

// FlyBehaviour inherits from GenericBehaviour. This class corresponds to the flying behaviour.
public class FlyBehaviour : GenericBehaviour
{
	public GameObject skeleton;
	public GameObject mesh;
	public GameObject skeleton1;
	public GameObject mesh1;
	public TrailRenderer trail1;
	public TrailRenderer trail2;
	public string flyButton = "Fly";              // Default fly button.
	public string jumpButton = "Jump";              // Default jump button.
	public float holdTimer = 0f;
	public float downwardForce = 5f;
	public float flySpeed = 4.0f;                 // Default flying speed.
	public float sprintFactor = 2.0f;             // How much sprinting affects fly speed.
	public float flyMaxVerticalAngle = 60f;       // Angle to clamp camera vertical movement when flying.

	private int flyBool;                          // Animator variable related to flying.
	private bool fly = false;                     // Boolean to determine whether or not the player activated fly mode.
	private CapsuleCollider col;                  // Reference to the player capsule collider.
	public float ver;
	private Rigidbody rb;

	// Start is always called after any Awake functions.
	void Start()
	{
		rb = behaviourManager.GetRigidBody;
		// Set up the references.
		flyBool = Animator.StringToHash("Fly");
		col = this.GetComponent<CapsuleCollider>();
		// Subscribe this behaviour on the manager.
		behaviourManager.SubscribeBehaviour(this);
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		if (Input.GetButton(jumpButton))
		{
			if (holdTimer < 1f) holdTimer += Time.deltaTime;
		}
		else
		{
			if (holdTimer > 0f && !fly) holdTimer -= Time.deltaTime * 2;
		}

		if (holdTimer > 0.5) fly = true;
		if (Input.GetButtonDown(jumpButton) || behaviourManager.IsGrounded()) 
		{
			holdTimer = 0f;
			fly = false;
		}

		ver = Input.GetAxis("Vertical");
		ver = 1;
		// Toggle fly by input, only if there is no overriding state or temporary transitions.
		// if (holdTimer > 0.8f && !behaviourManager.IsOverriding() 
		// 	&& !behaviourManager.GetTempLockStatus(behaviourManager.GetDefaultBehaviour))
		// {
		// 	fly = !fly;

		// 	// Force end jump transition.
		// 	behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);

			// Obey gravity. It's the law!
			// behaviourManager.GetRigidBody.useGravity = !fly;

			// Player is flying.
			if (fly)
			{
				// Register this behaviour.
				behaviourManager.RegisterBehaviour(this.behaviourCode);
				behaviourManager.GetCamScript.maxVerticalAngle = 10f;
				behaviourManager.UnlockTempBehaviour(behaviourManager.GetDefaultBehaviour);
				trail1.enabled = true;
				trail2.enabled = true;
				skeleton.SetActive(false);
				mesh.SetActive(false);
				skeleton1.SetActive(true);
				mesh1.SetActive(true);
			}
			else
			{
				// Set collider direction to vertical.
				col.direction = 1;
				// Set camera default offset.
				behaviourManager.GetCamScript.ResetTargetOffsets();
				behaviourManager.GetCamScript.maxVerticalAngle = 60f;
				// Unregister this behaviour and set current behaviour to the default one.
				behaviourManager.UnregisterBehaviour(this.behaviourCode);
				trail1.enabled = false;
				trail2.enabled = false;
				skeleton.SetActive(true);
				mesh.SetActive(true);
				skeleton1.SetActive(false);
				mesh1.SetActive(false);
			}
		// }

		// Assert this is the active behaviour
		// fly = fly && behaviourManager.IsCurrentBehaviour(this.behaviourCode);

		// Set fly related variables on the Animator Controller.
		behaviourManager.GetAnim.SetBool(flyBool, fly);
	}

	// This function is called when another behaviour overrides the current one.
	public override void OnOverride()
	{
		// Ensure the collider will return to vertical position when behaviour is overriden.
		col.direction = 1;
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Set camera limit angle related to fly mode.
		behaviourManager.GetCamScript.SetMaxVerticalAngle(flyMaxVerticalAngle);
		// behaviourManager.GetV = 1;

		// Call the fly manager.
		FlyManagement(behaviourManager.GetH, 1);
		behaviourManager.GetRigidBody.AddForce(0, downwardForce, 0, ForceMode.Acceleration);
	}
	// Deal with the player movement when flying.
	void FlyManagement(float horizontal, float vertical)
	{
		// Add a force player's rigidbody according to the fly direction.
		Vector3 direction = Rotating(horizontal, vertical);
		behaviourManager.GetRigidBody.AddForce(direction * (flySpeed * 100), ForceMode.Acceleration);
		//  * (behaviourManager.IsSprinting() ? sprintFactor : 1)
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
		// Camera forward Y component is relevant when flying.
		forward = forward.normalized;

		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		// Calculate target direction based on camera forward and direction key.
		Vector3 targetDirection = forward + right * horizontal;

		// Rotate the player to the correct fly position. // behaviourManager.IsMoving() && 
		if ((targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);

			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}

		// Player is flying and idle?
		// if (!(Mathf.Abs(horizontal) > 0.2 || Mathf.Abs(vertical) > 0.2))
		// {
		// 	// Rotate the player to stand position.
		// 	// behaviourManager.Repositioning();
		// 	// Set collider direction to vertical.
		// 	col.direction = 1;
		// }
		// else
		// {
		// 	// Set collider direction to horizontal.
		// 	col.direction = 2;
		// }
			// col.direction = 2;

		// Return the current fly direction.
		return targetDirection;
	}
}
