using UnityEngine;
using System.Collections;
using System;


public class SpellCaster : MonoBehaviour {

	public float Speed = 10;
//	bool _targetSet = false;
//	Vector3 _direction;
	string _spellTypeName;
	ThirdPersonCamera _camera;
	Transform _target;
	Vector3 _hitOffset;
	Vector3 _startPos;
	float _timeUntilHit;
	float _currentTime;

	GameObject _spellHitPrefab;

	public void Initialize(Vector3 targetPos, Transform target, string spellTypeName, GameObject spellHitPrefab)
	{

		_camera = ThirdPersonController.MyPlayer.gameObject.GetComponent<ThirdPersonCamera>();
		_target = target;
		_hitOffset = targetPos - _target.position;
		_startPos = transform.position;
	//	_camera.distance *= 1.5f;

		_spellHitPrefab = spellHitPrefab;
		UnityEngine.Random.seed = 1;
		_spellTypeName = spellTypeName;
//		_direction = (targetPos-transform.position).normalized;
//		_targetSet = true;

		float dist = (transform.position - targetPos).magnitude;
		_timeUntilHit = dist / Speed;
//		LeanTween.move(gameObject,targetPos,time).setOnComplete ( () => {
//			OnHitTarget(target);
//		});

	}



	void Update()
	{
		if (_target == null)
			return;

		_currentTime += Time.deltaTime;

		float percent = _currentTime / _timeUntilHit;

		Vector3 targetPos = _target.position + _hitOffset;
		transform.position = Vector3.Lerp(_startPos,targetPos,percent);

		if (percent >= 1)
		{
			OnHitTarget(_target);
			_target = null;
		}
	}

	void OnHitTarget(Transform target)
	{
//		_camera.Deactivate();

	//	Type currentType = Compiler.Instance.GetCurrentType();
		
		if (target.gameObject.isStatic == false && _spellTypeName != null && Compiler.CompiledTypes.ContainsKey(_spellTypeName))
		{
			collider.enabled = false;

	//		Debug.Log("adding type: " + currentType.Name + " to : " + collider.transform.name);
			if (target.GetComponent(_spellTypeName) != null)
				Destroy(target.gameObject.GetComponent(_spellTypeName));


			// kill the child projectile effect
			Destroy(transform.GetChild(0).gameObject);

			// start the hit effect
			Instantiate(_spellHitPrefab,transform.position,Quaternion.identity);

			Component spell = target.gameObject.AddComponent(Compiler.CompiledTypes[_spellTypeName]);
			target.gameObject.SendMessage("OnAttachedSpell",spell,SendMessageOptions.DontRequireReceiver);
			StartCoroutine(KillSpellAfterDelay(3.0f,spell));

//			if (target.rigidbody == null)
//				target.gameObject.AddComponent<Rigidbody>();

			if (target.GetComponent<MeshCollider>() != null)
				target.GetComponent<MeshCollider>().convex = true;
			Destroy(rigidbody);

		}


//		_targetSet = false;
	}

	IEnumerator KillSpellAfterDelay(float delay, Component spell)
	{
		yield return new WaitForSeconds(delay);

	//	_camera.Activate();
	//	_camera.distance *= .75f;
	//	_camera.SetTarget(ThirdPersonController.MyPlayer.transform);
		Destroy(spell);
		Destroy(gameObject);


	}

}
