using UnityEngine;
using System.Collections;

public class SavedSpellsPopup : MonoBehaviour {

	UIPopupList _popup;

	// Use this for initialization
	void Start () {
		_popup = gameObject.GetComponent<UIPopupList>();
		LoadSpells();

	}

	void OnCompiledSpell(string spellName)
	{

		LoadSpells();
	}

	void LoadSpells()
	{
		_popup.items = new System.Collections.Generic.List<string>( PlayerPrefsX.GetStringArray("SavedSpells") );
	}

	public void OnSpellSelected()
	{
		Debug.Log("selected");
		if (_popup != null && _popup.value != null)
			CodeInput.Instance.OnLoadSavedSpell(_popup.value);
		else
			Debug.Log("popup null");

	}
	

}
