using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class CatController : MonoBehaviour {


	public AnimationClip IdleClip;
	public AnimationClip WalkClip;
	public AnimationClip AttackClip;

	public GameObject LaserHitParticlePrefab;
	public WSP_LaserBeamWS[] Lasers;
	public float VisibiltyRadius = 200;
	public float MoveSpeed = 2;
	public float RotationSpeed = .5f;
	public int LaserDamage = 5;
	public Animation CatAnimation;

	AnimalState _currentState;
	List<Transform> _targets = new List<Transform>();

	int _team;

	// Use this for initialization
	void Start () {

		_team = GetComponent<team>().teamnumber;

		FindTargets();

	}

	
	// Update is called once per frame
	void Update () 
	{
		Transform target = CurrentTarget();

		if (target == null)
		{
			SetState(AnimalState.Idle,IdleClip);

			FindTargets();
			return;
		}

		if (_currentState == AnimalState.Idle)
		{
			SetState(AnimalState.Walk,WalkClip);
		}

		if (_currentState == AnimalState.Walk)
		{
			
			Quaternion targetOrientation = Quaternion.LookRotation(target.position - transform.position,Vector3.up);
			
			rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation,targetOrientation,Time.deltaTime * RotationSpeed);
			
			rigidbody.MovePosition(rigidbody.position + transform.forward * Time.deltaTime * MoveSpeed);


			FireWhenReady();
		}

	}

	void FireWhenReady()
	{
		Transform target = CurrentTarget();

		if (target == null || Lasers.Length == 0)
			return;

		RaycastHit hit;

		float maxFireAngle = 10;
		bool isLookingAtTarget = Vector3.Angle(transform.forward,target.position - transform.position) < maxFireAngle;
		bool isSomethingInTheWay = !(Physics.Raycast(Lasers[0].transform.position,target.position-transform.position,out hit) && hit.transform == target);

		if (isLookingAtTarget && isSomethingInTheWay == false) 
			StartCoroutine( Fire(target) );
	}

	IEnumerator Fire(Transform target)
	{
		float animTime = .5f;

		SetState(AnimalState.Attacking,AttackClip,animTime,false);
		audio.Play();

		yield return new WaitForSeconds(animTime);


		foreach (WSP_LaserBeamWS beam in Lasers)
		{
			beam.CurrentTarget = target;
			beam.FireLaser();

			Instantiate(LaserHitParticlePrefab,target.position,Quaternion.identity);

			_targets.Remove(target);

		}

		yield return new WaitForSeconds(1);

		if (target.GetComponent<health>() != null)
			target.GetComponent<health>().dead = true;


		SetState(AnimalState.Walk,WalkClip,animTime,true);

	}

	Transform CurrentTarget()
	{
		if (_targets.Count > 0)
			return _targets[0];
		else 
			return null;
	}

	void SetState(AnimalState state, AnimationClip clip)
	{
		SetState(state,clip,.3f,false);
	}

	void SetState(AnimalState state, AnimationClip clip, float fadeTime, bool changeStateAfterAnimComplete)
	{
		if (_currentState != state)
		{
			CatAnimation.CrossFade(clip.name,fadeTime);

			if (changeStateAfterAnimComplete)
				StartCoroutine(ChangeStateAfterDelay(fadeTime,state));
			else
				_currentState = state;
		}
		
	}

	IEnumerator ChangeStateAfterDelay(float delay, AnimalState state)
	{
		yield return new WaitForSeconds(delay);
		_currentState = state;
	}

	void FindTargets()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position,VisibiltyRadius);
		
		foreach (Collider collider in colliders)
		{
			if (collider.gameObject.GetComponent<team>() != null && collider.gameObject.GetComponent<team>().teamnumber != _team)
			{
				if (collider.gameObject.GetComponent<health>() != null && collider.gameObject.GetComponent<health>().dead == false)
					_targets.Add(collider.transform);
				
			}
		}
	}
}
