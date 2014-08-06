using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkConnect : Photon.MonoBehaviour
{

    /*
     * We want this script to automatically connect to Photon and to enter a room.
     * This will help speed up debugging in the next tutorials.
     * 
     * In Awake we connect to the Photon server(/cloud).
     * Via OnConnectedToPhoton(); we will either join an existing room (if any), otherwise create one. 
     */

	public UILabel StatusLabel;
	public GameObject WizardPrefab;

	public bool JumpToLevel;
	public GameObject JumpToLevelObj;

	public string[] WizardNames;

    void Start()
    {

		PhotonNetwork.playerName = PlayerPrefs.GetString("PlayerName", "Guest" + Random.Range(1, 9999) );


		if (PhotonNetwork.connected == true)
			OnJoinedRoom();
		else
   		    PhotonNetwork.ConnectUsingSettings("1.0");
    }


//    void OnGUI()
//    {
//        //Check connection state..
//        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
//        {
//            //We are currently disconnected
//            GUILayout.Label("Connection status: Disconnected");
//
//            GUILayout.BeginVertical();
//            if (GUILayout.Button("Connect"))
//            {
//                //Connect using the PUN wizard settings (Self-hosted server or Photon cloud)
//                PhotonNetwork.ConnectUsingSettings("1.0");
//            }
//            GUILayout.EndVertical();
//        }
//        else
//        {
//            //We're connected!
//            if (PhotonNetwork.connectionState == ConnectionState.Connected)
//            {
//                GUILayout.Label("Connection status: Connected");
//                if (PhotonNetwork.room != null)
//                {
//                    GUILayout.Label("Room: " + PhotonNetwork.room.name);
//                    GUILayout.Label("Players: " + PhotonNetwork.room.playerCount + "/" + PhotonNetwork.room.maxPlayers);
//
//                }
//                else
//                {
//                    GUILayout.Label("Not inside any room");
//                }
//
//                GUILayout.Label("Ping to server: " + PhotonNetwork.GetPing());
//            }
//            else
//            {
//                //Connecting...
//                GUILayout.Label("Connection status: " + PhotonNetwork.connectionState);
//            }
//        }
//    }

    private bool receivedRoomList = false;

	void OnJoinedRoom()
	{

//		int[] numInTeam = new int[2];
//		int team;
//
//		foreach (PhotonPlayer photonPlayer in PhotonNetwork.otherPlayers)
//		{
//			if (photonPlayer.customProperties.ContainsKey("Team") == false)
//			{
//				Debug.Log("Error: player does not have team assigned");
//				continue;
//			}
//
//
//			numInTeam[(int)photonPlayer.customProperties["Team"]]++;
//
//		}

		StatusLabel.enabled = false;
//		team = (numInTeam[0] > numInTeam[1]) ? 1 : 0;
//		Hashtable playerHash = PhotonNetwork.player.customProperties;
//		playerHash["Team"] = team;
//
//		PhotonNetwork.player.SetCustomProperties(playerHash);
//
//		Hashtable roomHash = PhotonNetwork.room.customProperties;

//		if (roomHash["StartTime"] == null)
//			roomHash["StartTime"] = PhotonNetwork.time;

//		PhotonNetwork.room.SetCustomProperties(roomHash);

		Debug.Log("joined room " + PhotonNetwork.playerList.Length);
//		vp_FPSPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPSPlayer>();
//		player.OnPlayerConnected(team);
	//	PhotonNetwork.Instantiate("FemElementalist",new Vector3(2.35582f,0.9143231f,-6.209982f), Quaternion.identity,0);

		Debug.Log("instantiating: " + WizardNames[1]);
	    GameObject player = (GameObject)PhotonNetwork.Instantiate(WizardNames[PhotonNetwork.playerList.Length-1],new Vector3(2.35582f,1.9143231f,-6.209982f), Quaternion.identity,0);
	//	PhotonNetwork.Instantiate(WizardNames[1],new Vector3(2.35582f,1.9143231f,-6.209982f), Quaternion.identity,0);
		GameObject playerPosTransform = GameObject.Find(WizardNames[PhotonNetwork.playerList.Length-1] + "Position");

		if (playerPosTransform != null)
		{
			player.transform.position = playerPosTransform.transform.position;
			player.transform.localScale = playerPosTransform.transform.localScale;
			player.transform.rotation = playerPosTransform.transform.rotation;
		}

		if (JumpToLevel)
		{
			StartCoroutine(JumpToLevelRoutine(player));
		}
	}

	IEnumerator JumpToLevelRoutine(GameObject player)
	{
		yield return new WaitForSeconds(.3f);
		JumpToLevelObj.SetActive(true);
		player.transform.position = new Vector3(JumpToLevelObj.transform.position.x,player.transform.position.y,JumpToLevelObj.transform.position.z);

	}

    void OnConnectedToPhoton()
    {
        StartCoroutine(JoinOrCreateRoom());
    }

    void OnDisconnectedFromPhoton()
    {
        receivedRoomList = false;
    }
	

    
    /// <summary>
    /// Helper function to speed up our testing: 
    /// - after connecting to Photon, check for active rooms and join the first if possible
    /// - if no roomlist was found within 2 seconds: Create a room
    /// </summary>
    /// <returns></returns>
    IEnumerator JoinOrCreateRoom()
    {
        float timeOut = Time.time + 2;
        while (Time.time < timeOut && !receivedRoomList)
        {
            yield return 0;
        }
        //We still didn't join any room: create one
        if (PhotonNetwork.room == null){
            string roomName = "TestRoom"+Application.loadedLevelName;
			PhotonNetwork.CreateRoom(roomName);//, true, true, 16);
			Debug.Log("creating room...");
        }
    }
    
    /// <summary>
    /// Not used in this script, just to show how list updates are handled.
    /// </summary>
    void OnReceivedRoomListUpdate()
    {
        Debug.Log("We received a room list update, total rooms now: " + PhotonNetwork.GetRoomList().Length);

		StatusLabel.text = "found a room";
        string wantedRoomName = "TestRoom" + Application.loadedLevelName;
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            if (room.name == wantedRoomName)
            {
                PhotonNetwork.JoinRoom(room.name);
                break;
            }
        }
        receivedRoomList = true;
    }
}
