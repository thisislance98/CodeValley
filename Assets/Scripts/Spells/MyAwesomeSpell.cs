using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MyAwesomeSpell : MonoBehaviour {
	
	
	void OnSpellHitPosition(Vector3 pos)
	{
		
		GameObject prefab = (GameObject)Resources.Load("Worm",typeof(GameObject));
		
		GameObject player = GameObject.FindWithTag("Player");

		Vector3 forward = player.transform.position - pos;
		Quaternion orientation = Quaternion.LookRotation(forward,Vector3.up);

		Instantiate(prefab,pos,orientation);

	}

	
}
