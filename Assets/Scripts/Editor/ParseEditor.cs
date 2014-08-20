using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Threading.Tasks;
using Parse;

[ExecuteInEditMode]
public class ParseEditor : MonoBehaviour {

	public static ParseEditor Instance;

	void Update()
	{
		if (Instance == null)
		{
			Debug.Log("Setting instance");
	//		ParseClient.Initialize("XThs1FrTSI9VSTgJQ2ShMiD5MtE0iVV44psWnTW2","4J9iTOF1IcWRjB29GSX41fEcF8YtxLi7Qvp552eX");
			Instance = this;
		}
	}

//	// Use this for initialization
//	void Start () {
//		Debug.Log("starting");
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		Debug.Log("update");
//
//	}

	public void DoTest()
	{

	}


	IEnumerator TestRoutine()
	{
		Debug.Log("starting test");

		ParseObject obj = new ParseObject("Test");
		obj["someKey"] = "data";
		Task task = obj.SaveAsync();

		for (int i=0; i < 10; i++)
		{
			Debug.Log("saving");

			yield return new WaitForSeconds(1);


			if (task.IsCompleted)
			{
				Debug.Log("saved"); 
				break;
			}

		}


	}

	[MenuItem("Assets/Test")]
	static void ExportResourceNoTrack () {

		Debug.Log("calling request");
		WWWForm form = new WWWForm();
		
		string jsonString = "{ score:100 }";
		Hashtable headers = new Hashtable();
		headers.Add("Content-Type", "application/json");
		headers.Add("X-Parse-Application-Id", "XThs1FrTSI9VSTgJQ2ShMiD5MtE0iVV44psWnTW2");
		headers.Add("X-Parse-REST-API-Key","DocYZL440ZHTom12APgvy0BpnZFIN7e9TdMP1qAn");
		
		form.AddField("score","23432442");
		//	form.AddBinaryData("theFile",System.Text.Encoding.UTF8.GetBytes("some text"));
		
		WWW www = new WWW("https://api.parse.com/1/classes/GameScore",System.Text.Encoding.UTF8.GetBytes(jsonString),headers);
		
		while (www.isDone == false)
		{
			//do nothing
		}
		
		if (www.error == null || www.error == string.Empty)
			Debug.Log("success: " + www.text);
		else
			Debug.Log("fail with error: " + www.error);

	}
}
