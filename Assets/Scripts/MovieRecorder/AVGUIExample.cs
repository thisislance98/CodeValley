using UnityEngine;
using System.Collections;

public class AVGUIExample : MonoBehaviour {
	AVREC avREC;
	
	void Start()
	{
		avREC = (GetComponent(typeof(AVREC)) as AVREC);
		
		Application.runInBackground = true;
	}
	
	void OnGUI()
	{
		
		if(avREC.IsFinalizing())
			GUI.Label(new Rect(Screen.width * 0.5f - 150.0f, 1.0f, 300.0f, 30.0f), "FINALIZING VIDEO.. PLEASE WAIT..");
		
		else if(avREC.IsRecording())
		{
			if(GUI.Button(new Rect(Screen.width * 0.5f - 15.0f, 1.0f, 100.0f, 30.0f), "Stop REC"))
				avREC.StopREC();
		}
		else if(GUI.Button(new Rect(Screen.width * 0.5f - 15.0f, 1.0f, 100.0f, 30.0f), "Start REC"))
			avREC.StartREC("Movies/test.mp4", false, 24, true);
		
		else if(GUI.Button(new Rect(Screen.width * 0.5f - 15.0f + 100.0f, 1.0f, 100.0f, 30.0f), "..with GUI"))
			avREC.StartREC("Movies/test.mp4", true, 24, true);
	}
}
