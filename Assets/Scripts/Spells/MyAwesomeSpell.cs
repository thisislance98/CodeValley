using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeValley;

public class MyAwesomeSpell : Spell {

//	Collider[] cols;

	void Start()
	{
//		cols = Physics.OverlapSphere(transform.position,100);

	}
	// Update is called once per frame
	void Update () {

		transform.Rotate(0,10,0);
	
//		foreach(Collider col in cols)
//		{
//			if (col.transform.name == "Terrain" || col.transform.tag == "Player")
//				continue;
//
//			col.transform.position += (transform.position - col.transform.position) * Time.deltaTime;
//		}

	
	}

}
