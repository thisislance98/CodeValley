using UnityEngine;
using System.Collections;

public class CodeInput : MonoBehaviour {

	public TextAsset CodeFile;
   UIInput _inputObj;

	// Use this for initialization
	void Start () {
	
		_inputObj = transform.GetComponent<UIInput>();
		_inputObj.value = CodeFile.text;
		_inputObj.label.text = CodeFile.text;

	//	Compiler.Instance.CompileSpell(_inputObj.label.text);
	}
	
	public void OnCompileTouch()
	{
		string code = _inputObj.label.text;

		Compiler.Instance.CompileSpell(code,ThirdPersonController.MyPlayer.photonView.viewID);


	}


}
