using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    public Text infoText;
    public RectTransform[] roomRects;
    public Button createRoomButton;
    public LobbyRoom lobbyRoomPrefab;
    private List<LobbyRoom> lobbyRooms;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
    }
    private void Start()
    {
        infoText.text = "마스터 서버에 접속중...";
        createRoomButton.interactable = false;
        PhotonNetwork.GameVersion = 1.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        infoText.text = "마스터 서버 접속 성공";
        createRoomButton.interactable = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        infoText.text = "오프라인 : 마스터서버에 접속 재시도...";
        createRoomButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 4} );
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
    public void JoinRoom(string roomName)
    {
        infoText.text = $"{roomName}에 참가중...";
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(LobbyRoom lobbyRoom in lobbyRooms)
        {
            Destroy(lobbyRoom.gameObject);
        }
        lobbyRooms.Clear();

        int activeRoomIndex = 0;
        foreach(RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) continue;

            LobbyRoom lobbyRoom = Instantiate(lobbyRoomPrefab);
            lobbyRoom.SetUp(this, roomInfo);

            RectTransform lobbyRect = lobbyRoom.GetComponent<RectTransform>();
            lobbyRect.SetParent(roomRects[activeRoomIndex], false);
            lobbyRect.anchoredPosition = Vector2.zero;

            activeRoomIndex++;

            if (activeRoomIndex >= roomRects.Length) break;
        }
    }
}
