using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public float RunSpeed;
	public float WalkSpeed;
	public float RotateSpeed;
	public float AttackDistance;
	public float ChaseDistance;
	public float PatrolRadius;
	public string[] AttackAnimations;
	public string TargetTag = "Player";


	Vector3 _startPos;
	Vector3 _currentPatrolDir;
	Transform _target;
	CharacterController _controller;
	AIState _state;

	enum AIState
	{
		NoTarget,
		ChaseTarget,
		AttackTarget
	}
	

	// Use this for initialization
	IEnumerator Start () {
	
		_controller = GetComponent<CharacterController>();
		OnNoTarget();
		_startPos = transform.position;
		SetNewPatrolDirection();

		// find the target
		while (_target == null)
		{
			GameObject targetObj = GameObject.FindGameObjectWithTag(TargetTag);

			if (targetObj != null)
			{
				_target = targetObj.transform;

			}

			yield return new WaitForSeconds(.5f);

		}
	}


	// Update is called once per frame
	void Update () 
	{
		UpdateStates();

		Vector3 targetDir = GetTargetDir();
		Vector3 moveDelta = transform.forward * GetSpeed() * Time.deltaTime;

		if (_controller.isGrounded == false)
		{
			moveDelta.y = Physics.gravity.y * Time.deltaTime;
		}

		_controller.Move(moveDelta);
		transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetDir,Vector3.up), Time.deltaTime*RotateSpeed);

	}

	void UpdateStates()
	{
		switch (_state) {

		case AIState.NoTarget:
			if (_target != null && Vector3.Distance(transform.position,_target.position) < ChaseDistance)
				ChaseTarget();


			break;

		case AIState.ChaseTarget:

			float dist = Vector3.Distance(_target.position,transform.position);

			if (dist < AttackDistance)
				AttackTarget();
			else if (dist > ChaseDistance)
				OnNoTarget();
			break;
			
		case AIState.AttackTarget:

			if (animation.isPlaying == false)
				animation.Play(AttackAnimations[Random.Range(0,AttackAnimations.Length)]);
			if (Vector3.Distance(_target.position,transform.position) > AttackDistance)
				ChaseTarget();

			break;
			
		}

	}

	void OnNoTarget()
	{
		_state = AIState.NoTarget;

		animation.CrossFade("walk");
	}

	void AttackTarget()
	{
		_state = AIState.AttackTarget;

		animation.Play(AttackAnimations[Random.Range(0,AttackAnimations.Length)]);
	}
	

	void ChaseTarget()
	{
		_state = AIState.ChaseTarget;

		animation.CrossFade("run");
	}

	void SetNewPatrolDirection()
	{
		Vector3 patrolPos = Random.insideUnitSphere * PatrolRadius;
		_currentPatrolDir = patrolPos - transform.position;
		_currentPatrolDir.y = 0;
	}

	Vector3 GetTargetDir()
	{
		Vector3 lookDir = Vector3.zero;
		
		switch (_state) {
			
		case AIState.NoTarget:
			lookDir = _currentPatrolDir;	
			break;
			
		case AIState.ChaseTarget:
			lookDir = _target.position - transform.position;
			break;
			
		case AIState.AttackTarget:
			lookDir = _target.position - transform.position;
			break;
			
		}
		
		lookDir.y = 0;
		
		return lookDir;
		
	}

	float GetSpeed()
	{
		switch (_state) {

		case AIState.NoTarget:
			return WalkSpeed;	

		case AIState.ChaseTarget:
			return RunSpeed;

		case AIState.AttackTarget:
			return 0;

		}

		return 0;

	}
}
