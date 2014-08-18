using UnityEngine;
using System.Collections;


public enum AnimalState
{
	Idle=0,
	Walk=1,
	Run=2,
	Attacking=3,
}

public class AnimalController : MonoBehaviour {


	public float ChangeStateTime = 7;
	public Transform BoundsObj;

	AnimalState _currentState;

	public float RotationSpeed = .5f;
	float[] _stateSpeeds = {0,10,20};
	float _currentSpeed;
	Animator _animator;
	Quaternion _targetOrientation;
	CharacterController _controller;
	Bounds _bounds;
	

	public AnimalState CurrentState {
		get {return _currentState; }
				
		set {

			_animator.ResetTrigger(_currentState.ToString());
			_currentState = value;
			_currentSpeed = _stateSpeeds[(int)_currentState];

	//		Debug.Log("setting state: " + _currentState.ToString());

			_animator.SetTrigger(_currentState.ToString());
			
		}
	}

	// Use this for initialization
	void Start () {

		_bounds = BoundsObj.collider.bounds;
	
		Destroy(BoundsObj.gameObject);


		_controller = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();

		CurrentState = AnimalState.Idle;

		StartCoroutine(ChangeToRandomState(ChangeStateTime));
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 velocity = (transform.forward * _currentSpeed * Time.deltaTime );

		if (_controller.isGrounded == false)
			velocity += Physics.gravity * Time.deltaTime ;

		_targetOrientation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x,_targetOrientation.eulerAngles.y,_targetOrientation.eulerAngles.z);

		if (CurrentState != AnimalState.Idle)
			transform.rotation = Quaternion.Slerp(transform.rotation,_targetOrientation,Time.deltaTime * RotationSpeed);

		_controller.Move(velocity);


		if (velocity.magnitude > _controller.velocity.magnitude )
		{
//			Debug.Log("changing directions");
			ChangeToRandomDirection();
		}

		if (_bounds.Contains(transform.position) == false)
			ChangeToDirectionInBounds();

	}



	void OnSensorHit()
	{

		ChangeToRandomDirection();
	}

	void ChangeToDirectionInBounds()
	{

		float x = Random.Range(_bounds.min.x,_bounds.max.x);
		float y = transform.position.y;
		float z = Random.Range(_bounds.min.z,_bounds.max.z);

		Vector3 dir = new Vector3(x,y,z);


//		Debug.Log("out of bounds new dir "  + dir);
		_targetOrientation = Quaternion.LookRotation((dir - transform.position).normalized,transform.up);
	}

	void ChangeToRandomDirection()
	{
//		Debug.Log("changing to random direction");
		_targetOrientation = transform.rotation * Quaternion.AngleAxis(Random.Range(90,270),transform.up);
	}

	IEnumerator ChangeToRandomState(float delay)
	{
		yield return new WaitForSeconds(delay);

		CurrentState = (AnimalState)Random.Range(0,3);

		StartCoroutine( ChangeToRandomState(delay) );

	}


}
