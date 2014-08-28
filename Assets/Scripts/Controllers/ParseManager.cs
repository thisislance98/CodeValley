using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Parse;

public class ParseManager : MonoBehaviour {

	public static ParseManager Instance;

	void Awake()
	{
		Instance = this;
//		ParseClient.Initialize("12zbQzuJZSFdRZ2TLTy195huFNPthww1gT0uSsgX","dkpQN45spUUwQ6Z8FWeYgk1BfuOVN2pbhwPtIa5i");



	}

	// Use this for initialization
	void Start () {

//		var query = ParseObject.GetQuery("GameScore")
//			.WhereEqualTo("playerName", "Dan Stemkoski");
//		query.FindAsync().ContinueWith(t =>
//		                               {
//			IEnumerable<ParseObject> results = t.Result;
//		});
	}
	

}
