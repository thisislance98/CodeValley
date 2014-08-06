#pragma strict


var strfun = "function Update() { transform.Translate(1,0,0); }";



//function Update()
//{
//	tranform.Translate(1,0,0);
//
//}
//
//
//
//
//");

function Start()
{
	Debug.Log("starting");
	eval(strfun);
	
	


}

//function Update()
//{
//	Debug.Log("updating");
////	transform.Translate(1,0,0);
//}

function Evaluate (code : String) {

//	transform.Translate(0,2,0);
	Debug.Log("evulating on " + transform.name);
//	try
//	{
//		eval(code);
//	}
//	catch (err)
//	{
//		Debug.Log ("got error: " + err);
//	
//	}
//	Destroy(this);
}