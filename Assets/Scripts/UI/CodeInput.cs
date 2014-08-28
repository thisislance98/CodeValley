using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CodeInput : MonoBehaviour {

//	public TextAsset CodeFile;

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

	void OnSpellLoaded(Spell spell)
	{
		_inputObj.value = spell.Code;
		_inputObj.label.text = spell.Code;
	}

	public void OnCompileTouch()
	{
		SpellManager.Instance.CompileSpell();

	}


}
