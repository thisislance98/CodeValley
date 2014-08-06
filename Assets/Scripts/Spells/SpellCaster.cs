using UnityEngine;
using System.Collections;
using System;


public class SpellCaster : MonoBehaviour {

	public float Speed = 10;
//	bool _targetSet = false;
//	Vector3 _direction;
	string _spellTypeName;
	ThirdPersonCamera _camera;

	GameObject _spellHitPrefab;

	public void Initialize(Vector3 targetPos, Transform target, string spellTypeName, GameObject spellHitPrefab)
	{
		_camera = ThirdPersonController.MyPlayer.gameObject.GetComponent<ThirdPersonCamera>();
		_camera.distance *= 1.5f;

		_spellHitPrefab = spellHitPrefab;
		UnityEngine.Random.seed = 1;
		_spellTypeName = spellTypeName;
//		_direction = (targetPos-transform.position).normalized;
//		_targetSet = true;

		float dist = (transform.position - targetPos).magnitude;
		float time = dist / Speed;
		LeanTween.move(gameObject,targetPos,time).setOnComplete ( () => {
			OnHitTarget(target);
		});


	}

//	void Update()
//	{
//		if (_targetSet == false)
//			return;
//
//		transform.position += _direction * Speed * Time.deltaTime;
//
//	}

	void OnHitTarget(Transform target)
	{
		_camera.Deactivate();

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
			StartCoroutine(KillSpellAfterDelay(2.0f,spell));

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

		_camera.Activate();
		_camera.distance *= .75f;
		_camera.SetTarget(ThirdPersonController.MyPlayer.transform);
		Destroy(spell);
		Destroy(gameObject);


	}

}
