using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	public void OnReset()
	{
		Application.LoadLevel (Application.loadedLevelName);

	}
}
