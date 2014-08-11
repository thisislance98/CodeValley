using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicWorld : MonoBehaviour {

	Dictionary<string,GameObject> _AllObjs = new Dictionary<string, GameObject>();

	public static DynamicWorld Instance;


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		AddChildrenAndDisableRecursive(gameObject);

	}
	

	void AddChildrenAndDisableRecursive(GameObject parent)
	{
		for (int i=0; i < parent.transform.childCount; i++)
		{
			GameObject child = parent.transform.GetChild(i).gameObject;

			AddChildrenAndDisableRecursive(child);
		}

		if (parent.transform.childCount == 0)
		{
			_AllObjs.Add(parent.name,parent);

			parent.SetActive(false);
		}
	}

	public GameObject ReplaceWithDynamic(GameObject staticObj)
	{
		if (_AllObjs.ContainsKey(staticObj.name) == false)
		{
			Debug.Log ("dynamic obj: " + staticObj.name + " not found");
			return null;
		}

		GameObject dynamicObj = _AllObjs[staticObj.name];
		
		if (dynamicObj == null)
			return null;
		
		staticObj.collider.enabled = false;
		Destroy(staticObj);
		
		dynamicObj.SetActive(true);
		
		return dynamicObj;
		
	}
}
