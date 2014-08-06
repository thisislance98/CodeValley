using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

	public GameObject Observer;

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("sensor hit");
		Observer.SendMessage("OnSensorHit");

	}
}
