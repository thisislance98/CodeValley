using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CodeInput : MonoBehaviour {

//	public TextAsset CodeFile;
	public GameObject OnCompileObserver;
	public UIPopupList SpellHitPopup;
	public UIPopupList SpellProjectilePopup;

	public UIInput SpellNameInput;
	UIInput _inputObj;

	public static CodeInput Instance;

	// Use this for initialization
	void Awake () {
	
		Instance = this;
		_inputObj = transform.GetComponent<UIInput>();
//		_inputObj.value = CodeFile.text;
//		_inputObj.label.text = CodeFile.text;


	//	Compiler.Instance.CompileSpell(_inputObj.label.text);
	}

	public UIInput GetInput()
	{
		return _inputObj;
	}

	public void OnLoadSavedSpell(string spellName)
	{
		string code = PlayerPrefs.GetString(spellName);

		_inputObj.value = code;
		_inputObj.label.text = code;

		Debug.Log("loading");
		SpellNameInput.value = spellName;
		SpellNameInput.label.text = spellName;

		if (PlayerPrefs.HasKey(spellName + "Hit"))
		    SpellHitPopup.GetComponent<SpellPopupList>().SetSpell(PlayerPrefs.GetString(spellName + "Hit"));

		if (PlayerPrefs.HasKey(spellName + "Projectile"))
			SpellProjectilePopup.GetComponent<SpellPopupList>().SetSpell(PlayerPrefs.GetString(spellName + "Projectile"));

		StartCoroutine(Compile());
	}

	public void OnCompileTouch()
	{
		StartCoroutine(Compile());

	}

	IEnumerator Compile()
	{
		while (ThirdPersonController.MyPlayer == null)
		{
			yield return new WaitForSeconds(.1f);
		}
		
		string code = _inputObj.label.text;
		string spellName = SpellNameInput.value;
		
		Compiler.Instance.CompileSpell(code,ThirdPersonController.MyPlayer.photonView.viewID);
		
		PlayerPrefs.SetString(spellName,code);
		
		List<string> savedSpells = new List<string>(PlayerPrefsX.GetStringArray("SavedSpells"));
		
		if (savedSpells.Contains(spellName) == false)
		{
			savedSpells.Add(spellName);
			PlayerPrefsX.SetStringArray("SavedSpells",savedSpells.ToArray());
		}

		PlayerPrefs.SetString(spellName + "Projectile", SpellProjectilePopup.value);
		PlayerPrefs.SetString(spellName + "Hit", SpellHitPopup.value);

		OnCompileObserver.SendMessage("OnCompiledSpell",spellName);
	}


}
