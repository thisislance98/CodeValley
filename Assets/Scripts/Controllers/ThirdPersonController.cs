using UnityEngine;
using System.Collections;
using System;

public class ThirdPersonController : Photon.MonoBehaviour {

	
	public Transform SpellStartTransform;


	public float JumpForce = 1000;
	public float walkMaxAnimationSpeed  = 0.75f;
	public float runMaxAnimationSpeed  = 1.0f;
	public float jumpAnimationSpeed = 1.15f;
	public float landAnimationSpeed = 1.0f;

	public float rotateSpeed = 500.0f;
	// The speed when walking
	public float walkSpeed = 2.0f;

	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 6.0f;

	// The gravity in controlled descent mode
	float _speedSmoothing = 10.0f;


	private string _currentSpellClassTypeName;

	// The current move direction in x-z
	private Vector3 _moveDirection = Vector3.zero;

	// The current x-z move speed
	private float _moveSpeed = 0.0f;

	private Vector3 _peerTargetPos;
	private Quaternion _peerTargetRotation;

	private Vector3 _lastPos;
	private Vector3 _lastMoveDirection;

	private Animator _animator;
	
	private bool isControllable = true;
	private Component _attachedSpell = null;

	public static ThirdPersonController MyPlayer;
	private bool _isCastingSpell = false;
	private Transform _highlightedObj = null;
	private Color _lastHighlightedColor;

	// RMFX Mod 
	// Suspend this character controller by setting isSuspended to true.
	public bool isSuspended = false;
	

	void Awake ()
	{
		_animator = GetComponent<Animator>();
		_moveDirection = transform.TransformDirection(Vector3.forward);

		_peerTargetPos = rigidbody.position;
		_peerTargetRotation = rigidbody.rotation;

		if (photonView.isMine)
			MyPlayer = this;
	}

	public void SetSpellClassTypeName(string name)
	{
		_currentSpellClassTypeName = name;
	}

	public void OnAttachedSpell(Component spell)
	{
		_attachedSpell = spell;

	}
	

	void UpdatePeer()
	{
		// if spell is a attached just move the target pos along with the actual
		if (_attachedSpell != null)
		{
			_peerTargetPos = rigidbody.position;
			_peerTargetRotation = rigidbody.rotation;
			return;
		}

		float lerpSpeed = 10;

		rigidbody.position = Vector3.Lerp(rigidbody.position,_peerTargetPos,Time.deltaTime*lerpSpeed);
		rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation,_peerTargetRotation,Time.deltaTime*lerpSpeed);
	}

	void FixedUpdate() {


		if (photonView.isMine == false)
		{
			UpdatePeer();
			return;
		}

		// don't allow controll if a spell is currently attached
		if (_attachedSpell != null || isSuspended || CodeInput.Instance.GetInput().isSelected)
		{
			return;
		}

		if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}

		if (Input.GetButtonDown ("Jump") && Math.Abs( rigidbody.velocity.y ) < .1f)
		{
			_animator.SetTrigger("Jump");
			rigidbody.AddForce(0,JumpForce,0,ForceMode.Impulse);
		}

		UpdateSmoothedMovementDirection();
		

		// Calculate actual motion
		Vector3 movement = _moveDirection * _moveSpeed;
		Quaternion rotation = Quaternion.LookRotation(_moveDirection,Vector3.up);

		PhotonNetwork.RPC(photonView,"SetSpeed",PhotonTargets.All,_moveSpeed);

		movement *= Time.deltaTime;

		if (movement.magnitude > 0) 
			rigidbody.MovePosition(rigidbody.position + movement);

		if (_moveDirection != _lastMoveDirection)
			rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation,rotation,Time.deltaTime*rotateSpeed);

		// check for spell cast
		if (Input.GetMouseButtonDown(0) && Utils.Instance.IsTouchingUI() == false && _isCastingSpell == false)
		{
			_isCastingSpell = true;
			Debug.Log("casting");
			
			StartCoroutine(CastSpell());
		}

		HighlightOnMouseOver();

		// update peers
		if (rigidbody.position != _lastPos)
			PhotonNetwork.RPC(photonView,"SetPeerTargetPos",PhotonTargets.OthersBuffered,rigidbody.position);

		if (_moveDirection != _lastMoveDirection)
			PhotonNetwork.RPC(photonView,"SetPeerTargetRotation",PhotonTargets.OthersBuffered,rigidbody.rotation);

		_lastPos = rigidbody.position;
		_lastMoveDirection = _moveDirection;
	}

	void HighlightOnMouseOver()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10000))
		{
//			Debug.Log("touching: " + hit.transform.name);

			if (hit.transform.renderer != null && hit.transform != _highlightedObj)
			{

				if (_highlightedObj != null)
					_highlightedObj.renderer.material.color = _lastHighlightedColor;

		
				_lastHighlightedColor = hit.transform.renderer.material.color;
				hit.transform.renderer.material.color = Color.green;
				_highlightedObj = hit.transform;
			}
		}
		else
		{
			if (_highlightedObj != null)
				_highlightedObj.renderer.material.color = _lastHighlightedColor;

			_highlightedObj = null;
		}
		
	}

	IEnumerator CastSpell()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10000))
		{
			Debug.Log("touching: " + hit.transform.name);

			PhotonNetwork.RPC(photonView,"PlayCastSpellAnimation",PhotonTargets.All,hit.point);

			yield return new WaitForSeconds(.3f);
		
			PhotonNetwork.RPC(photonView,"CastSpellRPC",PhotonTargets.All,hit.point,hit.transform.name);

			yield return new WaitForSeconds(.5f);


		}
		_isCastingSpell = false;
	}

	void UpdateSmoothedMovementDirection ()
	{
		Transform cameraTransform = Camera.main.transform;
		
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");


		Vector3 right = Quaternion.AngleAxis(90,Vector3.up) * forward;


		// Target direction relative to the camera
	//	Vector3 targetDirection = h * Vector3.right + v * Vector3.forward;

		Vector3 targetDirection = v * forward + h * right;
		
		
		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if (targetDirection != Vector3.zero)
		{
			//			// If we are really slow, just snap to the target direction
			//			if (_moveSpeed < walkSpeed * 0.5)
			//			{
			//				_moveDirection = targetDirection.normalized;
			//			}
			//			// Otherwise smoothly turn towards it
			//			else
			//			{
			_moveDirection = Vector3.RotateTowards(_moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
			
			_moveDirection = _moveDirection.normalized;
			//			}
		}
		
		// Smooth the speed based on the current target direction
		float curSmooth = _speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
		
		// Pick speed modifier
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			targetSpeed *= runSpeed;
		}
		else
		{
			targetSpeed *= walkSpeed;
		}
		
		_moveSpeed = Mathf.Lerp(_moveSpeed, targetSpeed, curSmooth);
		
	}

	void OnControllerColliderHit ( ControllerColliderHit hit )
	{
	//	Debug.DrawRay(hit.point, hit.normal);
		if (hit.moveDirection.y > 0.01) 
			return;
	}
	

	[RPC]
	void SetSpeed(float speed)
	{
		_animator.SetFloat("Speed",speed);

	}

	[RPC] 
	void SetPeerTargetPos(Vector3 targetPos)
	{
		if (_attachedSpell != null)
			return;

		_peerTargetPos = targetPos;
	}

	[RPC]
	void SetPeerTargetRotation(Quaternion targetRotation)
	{
		if (_attachedSpell != null)
			return;

		_peerTargetRotation = targetRotation;
	}


	[RPC]
	void CastSpellRPC(Vector3 hitPoint, string objName)
	{
		if (objName == "Terrain")
			return;


		_moveDirection = (hitPoint - rigidbody.position).normalized;
		_moveDirection.y = 0;
		rigidbody.rotation = Quaternion.LookRotation(_moveDirection,Vector3.up);

		GameObject target = GameObject.Find(objName);

		if (target.isStatic)
			target = DynamicWorld.Instance.ReplaceWithDynamic(target);

		if (target == null)
			return;

		Vector3 startPos = transform.position + transform.forward*3 + Vector3.up*4;
		GameObject spell = SpellManager.Instance.CastSpell(startPos,hitPoint,target.transform,_currentSpellClassTypeName);

	//	GetComponent<ThirdPersonCamera>().SetTarget(spell.transform);
	}
	
	[RPC]
	void PlayCastSpellAnimation(Vector3 target)
	{
		_animator.SetTrigger("CastSpell");


	}
}

