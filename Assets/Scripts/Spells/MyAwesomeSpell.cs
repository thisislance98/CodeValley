using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeValley;

public class MyAwesomeSpell : Spell {


	void Start()
	{
		if (gameObject.GetComponent<Rigidbody>() == false)
			gameObject.AddComponent<Rigidbody>();
	}

	void Update () {

	//	transform.Rotate(0,10,0);
	//	rigidbody.AddForce(10,0,0);


		transform.localScale -= Vector3.up * Time.deltaTime * .5f;
	}




}
