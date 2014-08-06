using UnityEngine;
using System.Collections;

public class Level1Gateway : MonoBehaviour {

	public GameObject NextLevel;
	public GameObject LastLevel;


	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag != "Player")
			return;


		for (int i=0; i < transform.childCount; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			LeanTween.move(child,transform.position + new Vector3(100,100,100),3);

		}

		NextLevel.SetActive(true);
		Vector3 nextLevelPos = NextLevel.transform.position;
		Vector3 nextLevelRotation = NextLevel.transform.rotation.eulerAngles;
		NextLevel.transform.rotation = Quaternion.Euler(0,0,0);
		NextLevel.transform.position = new Vector3(100,100,100);
		LeanTween.move(NextLevel,nextLevelPos,3).setOnComplete( () => {
			LeanTween.rotate(NextLevel,nextLevelRotation,3);

		});

	}

	void OnTriggerExit(Collider other)
	{		
		if (other.transform.tag != "Player")
			return;
		
		LeanTween.move(LastLevel,LastLevel.transform.position + new Vector3(1000,1000,1000),3).setOnComplete( ()=> {
			Destroy(LastLevel);
		});


	}


}
