using UnityEngine;
using System.Collections;

public class SleepOnAwake : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//	rigidbody.Sleep();
	}
	
	void Update()
	{
		float vel = rigidbody.velocity.magnitude;

		if (vel > .1f)
		{
			Vector3 newVel = rigidbody.velocity.normalized * .1f;
			rigidbody.velocity = newVel;
		}
	}
}
