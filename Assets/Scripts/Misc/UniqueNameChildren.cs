using UnityEngine;
using System.Collections;

public class UniqueNameChildren : MonoBehaviour {

	int tagIndex = 0;

	// Use this for initialization
	void Start () {
		TagChildrenRecursive(transform);
	}

	void TagChildrenRecursive(Transform parent)
	{
		for (int i=0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);

			TagChildrenRecursive(child);

		}

		parent.name = parent.name + " - " + tagIndex;

		tagIndex++;

	}
	

}
