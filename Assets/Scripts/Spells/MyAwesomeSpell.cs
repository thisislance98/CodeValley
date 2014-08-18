using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MyAwesomeSpell : MonoBehaviour {

	static Camera lastCamera = null;

	void Start()
	{
		if (lastCamera != null)
			Destroy(lastCamera);

		Camera camera = gameObject.AddComponent<Camera>();

		camera.fieldOfView = 120;

		camera.rect = new Rect(.5f,.5f,.5f,.5f);

		lastCamera = camera;
	}

	
	
}
