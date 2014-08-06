using UnityEngine;
using System.Collections;

public class HideShowButton : MonoBehaviour {

	public UILabel Label;
	public GameObject[] Objects;
	bool _isShowing = true;


	public void ToggleShow()
	{
		_isShowing = !_isShowing;

		Label.text = (_isShowing) ? "Hide" : "Show";

		foreach(GameObject obj in Objects)
		{
			obj.SetActive(_isShowing);

		}

	}

}
