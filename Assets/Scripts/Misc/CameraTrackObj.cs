using UnityEngine;
using System.Collections;

public class CameraTrackObj : MonoBehaviour {

	public Transform Target;
	public float Speed = .5f;

	Vector3 _offset;


	// Update is called once per frame
	void Update () {

		if (Target == null)
		{
			GameObject target = GameObject.FindGameObjectWithTag("Player");


			if (target == null)
				return;
			else
			{
				Target = target.transform;
				_offset = Target.position - transform.position;
			}

		}

		Vector3 targetPos = Target.position - _offset;


		transform.position = Vector3.Slerp(transform.position,targetPos,Time.deltaTime * Speed);
	
	}
}
