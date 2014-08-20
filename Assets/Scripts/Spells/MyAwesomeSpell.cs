using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MyAwesomeSpell : MonoBehaviour {

	
	void OnSpellHitPosition(Vector3 pos)
	{
		Instantiate(Resources.Load("Kitty",typeof(GameObject)),pos,Quaternion.identity);

	}

	
	
}
