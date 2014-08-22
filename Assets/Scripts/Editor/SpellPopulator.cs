using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public class SpellPopulator : MonoBehaviour {

	[MenuItem("Spell Manager/Populate Spells &#s")]
	static void PopulateSpellsFromDirectories()	
	{
		SpellManager manager = GameObject.Find("SpellManager").GetComponent<SpellManager>();


		manager.SpellHitPrefabs.Clear();
		manager.SpellProjectilePrefabs.Clear();

		PopulateList(ref manager.SpellHitPrefabs,manager.HitSpellsDirectory);
		PopulateList(ref manager.SpellProjectilePrefabs,manager.ProjectileSpellsDirectory);
		
	}
	
	static void PopulateList(ref List<GameObject> list, string directoryPath)
	{

		string[] filePaths = Directory.GetFiles(directoryPath);
		Debug.Log("found objs: " + filePaths.Length);
		
		foreach (string filePath in filePaths)
		{
			GameObject obj =  AssetDatabase.LoadAssetAtPath(filePath,typeof(GameObject)) as GameObject;
			if (obj != null)
			{
				Debug.Log("obj name: " + obj.name);
				
				
				list.Add(obj);
			}
		}

		string[] directories = Directory.GetDirectories(directoryPath);

		foreach (string directory in directories)
		{
			PopulateList(ref list, directory);
		}
		
	}
}
