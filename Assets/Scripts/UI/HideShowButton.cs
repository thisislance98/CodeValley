using UnityEngine;
using System.Collections;

public class HideShowButton : MonoBehaviour {

	public bool HideOnStart = true;
	public UILabel Label;
	public GameObject[] NonSiblingObjects;
	bool _isShowing = true;

	void Start()
	{
		if (HideOnStart)
			ToggleShow();
	}

	public void ToggleShow()
	{
		_isShowing = !_isShowing;

		Label.text = (_isShowing) ? "Hide" : "Show";



		for (int i=0; i < transform.parent.childCount; i++)
		{
			GameObject obj = transform.parent.GetChild(i).gameObject;

			if (obj != gameObject)
				obj.SetActive(_isShowing);

		}

		foreach (GameObject obj in NonSiblingObjects)
		{
			obj.SetActive(_isShowing);
		}

	}

}
