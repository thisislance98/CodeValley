using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

	public GameObject Observer;

	void OnTriggerEnter(Collider other)
	{
		Observer.SendMessage("OnSensorHit");

	}
}
