using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class LobbyRoom : MonoBehaviour 
{
    private Button button;
    public Text roomNameText;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    
    public void SetUp(LobbySceneManager lobbySceneManager ,RoomInfo roomInfo)
    {
        roomNameText.text = roomInfo.Name.Substring(0, 5);
        button.onClick.AddListener(() => lobbySceneManager.JoinRoom(roomInfo.Name));
    }
}
