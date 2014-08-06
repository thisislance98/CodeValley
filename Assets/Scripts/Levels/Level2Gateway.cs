using UnityEngine;
using System.Collections;

public class Level2Gateway : MonoBehaviour {
	
	public TextMesh Title;
	// Update is called once per frame
	void Update () {

		if (renderer.material.color == Color.green)
		{
			StartCoroutine(OnLevelComplete());
		}
	
	}

	IEnumerator OnLevelComplete()
	{
		Title.text = "Thank You!";

		yield return new WaitForSeconds(3);

		Destroy(gameObject);
	}
}
