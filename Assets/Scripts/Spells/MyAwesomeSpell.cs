using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeValley;

public class MyAwesomeSpell : Spell {

	Transform player;

	void Start()
	{
//		if (gameObject.GetComponent<Rigidbody>() == false)
//			gameObject.AddComponent<Rigidbody>();

		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {

	//	transform.Rotate(0,10,0);
	//	rigidbody.AddForce(10,0,0);

		transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime*.4f);
	//	transform.localScale -= Vector3.up * Time.deltaTime * .5f;
	}




}
