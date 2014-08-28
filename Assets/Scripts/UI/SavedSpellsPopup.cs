using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

 
public class SavedSpellsPopup : MonoBehaviour {

	UIPopupList _popup;

	// Use this for initialization
	void Awake () {
		_popup = gameObject.GetComponent<UIPopupList>();

		Debug.Log("popup loaded: " + (_popup == null));
//		LoadSpells();

	}

//	void OnCompiledSpell(string spellName)
//	{
//
//		LoadSpells();
//	}

//	void LoadSpells()
//	{
//		string[] filePaths = Directory.GetFiles("Assets/Resources/Spells");
//
//		List<string> savedFiles = new List<string>();
//
//		foreach (string filePath in filePaths)
//		{
//			if (filePath.Contains(".meta"))
//			    continue;
//
//			string fileName = filePath.Split(new char[]{'/'})[3].Split(new char[]{'.'})[0];
//
//			savedFiles.Add(fileName);
//
//		}
//
//		List<string> allSpells = new List<string>(PlayerPrefsX.GetStringArray("SavedSpells"));
//		allSpells.AddRange(savedFiles);
//
//		_popup.items = allSpells;
//	}

	public void OnSpellNamesDownloaded(List<string> spellNames)
	{
		Debug.Log("got spell names");
		_popup.items = spellNames;
	}

	public void OnSpellSelected()
	{

		if (_popup != null && _popup.value != null)
			SpellManager.Instance.OnLoadSavedSpell(_popup.value);
		else
			Debug.Log("popup null");

	}
	

}
