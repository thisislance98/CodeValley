using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public Camera UICamera;

	public static Utils Instance;

	// Use this for initialization
	void Start () {
	
		Instance = this;
	}
	
	public bool IsTouchingUI()
	{
		Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		return Physics.Raycast(ray, out hit, 1000);

	}
}
