using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class NetworkTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DownloadSpells();
	}
	
	void DownloadSpells()
	{
		var query = ParseObject.GetQuery("Spell");


		Debug.Log("downloading");

		query.FindAsync().ContinueWith(t => {
			
			IEnumerable<ParseObject> results = t.Result;
			
			
			Debug.Log("got spells");
			foreach(ParseObject spellObj in results)
			{
				Spell spell = new Spell();
				
				spell.Name = spellObj.Get<string>("Name");
				spell.HitEffect = spellObj.Get<string>("HitEffect");
				spell.ProjectileEffect = spellObj.Get<string>("ProjectileEffect");
				spell.Code = spellObj.Get<string>("Code");
				spell.CanHitTerrain = spellObj.Get<bool>("CanHitTerrain");

			}

			
		});
	}
}
