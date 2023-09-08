using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	private string gameVersion = "1.0";
	[SerializeField] string playerName = "";
	[SerializeField] TMP_Text TotalPlayerInLobbyText;
	[SerializeField] GameObject CreateRoomPanel;
	[SerializeField] GameObject JoinRoomPanel;
	[SerializeField] GameObject LobbyPanel;
	[SerializeField] GameObject PlayGameButton;
	[SerializeField] GameObject CreatedRoomsStatusText;

	void Start()
	{
		ConnectToPhoton();
		if(playerName == "")
		{
			playerName = "Player" + Random.Range(100, 999);
		}
	}

	#region ConnectionHandler
	public void ConnectToPhoton()
	{
		if (PhotonNetwork.IsConnected)
		{
			Debug.Log("Already connected to Photon.");
			return;
		}

		PhotonNetwork.GameVersion = gameVersion;
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.NickName = playerName;
		PhotonNetwork.ConnectUsingSettings();

		Debug.Log("Connecting to Photon...");
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon's Master Server");
		PhotonNetwork.JoinLobby();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarning("Disconnected from Photon: " + cause.ToString());
	}

	#endregion

	#region CreateRoom

	public void CreateRoom(TMP_InputField RoomName)
	{
		if (!PhotonNetwork.IsConnected)
		{
			Debug.LogError("Not connected to Photon. Make sure you've set up the PUN settings.");
			return;
		}

		string roomName = RoomName.text + Random.Range(1, 1000);

		RoomOptions roomOptions = new RoomOptions
		{
			MaxPlayers = 10, 
			IsVisible = true,
			IsOpen = true
		};
		PhotonNetwork.CreateRoom(roomName, roomOptions);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Room created successfully!");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.LogError("Room creation failed: " + message);
	}

	#endregion

	#region JoinRoom

	public GameObject roomListItemPrefab;
	public Transform roomListParent;

	private List<RoomInfo> roomList = new List<RoomInfo>();

	public override void OnRoomListUpdate(List<RoomInfo> updatedRoomList)
	{
		roomList.Clear();
		Debug.Log("RoomListUpdated");

		roomList.AddRange(updatedRoomList);

		UpdateRoomListUI();
	}

	void UpdateRoomListUI()
	{
		foreach (Transform child in roomListParent)
		{
			Destroy(child.gameObject);
		}
		if(roomList.Count == 0)
		{
			CreatedRoomsStatusText.SetActive(true);
		}
		else
		{
			CreatedRoomsStatusText.SetActive(false);
		}

		foreach (RoomInfo room in roomList)
		{
			GameObject roomListItem = Instantiate(roomListItemPrefab);
			roomListItem.transform.SetParent(roomListParent, false);
			roomListItem.transform.GetChild(0).GetComponent<TMP_Text>().text = room.Name;
			roomListItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => JoinRoom(room.Name));
		}
	}

	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
		CreateRoomPanel.SetActive(false);
		JoinRoomPanel.SetActive(false);
		LobbyPanel.SetActive(true);
		ShowLobbyDetails();
		if(PhotonNetwork.IsMasterClient)
		{
			PlayGameButton.SetActive(true);
		}
		else
		{
			PlayGameButton.SetActive(false);
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		ShowLobbyDetails();
	}

	#endregion

	#region LobbyHandler

	public void ShowLobbyDetails()
	{
		if (PhotonNetwork.InRoom)
		{
			int totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
			TotalPlayerInLobbyText.text = "Total Players: " + totalPlayers;
		}
	}

	#endregion

	#region HandleSceneLoading

	public void LoadSceneForAllPlayers()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("Game");
			PhotonNetwork.CurrentRoom.IsVisible = false;
		}
	}
	#endregion
}
