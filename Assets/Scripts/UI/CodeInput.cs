using UnityEngine;
using System.Collections;

public class CodeInput : MonoBehaviour {

	public TextAsset CodeFile;

	UIInput _inputObj;

	public static CodeInput Instance;

	// Use this for initialization
	void Start () {
	
		Instance = this;
		_inputObj = transform.GetComponent<UIInput>();
		_inputObj.value = CodeFile.text;
		_inputObj.label.text = CodeFile.text;


	//	Compiler.Instance.CompileSpell(_inputObj.label.text);
	}

	public UIInput GetInput()
	{
		return _inputObj;
	}

	public void OnCompileTouch()
	{
		if (ThirdPersonController.MyPlayer == null)
			return;

		string code = _inputObj.label.text;

		Compiler.Instance.CompileSpell(code,ThirdPersonController.MyPlayer.photonView.viewID);


	}


}
