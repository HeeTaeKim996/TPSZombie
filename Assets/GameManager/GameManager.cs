using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public bool isUnconnectedForDevelop;
    public bool isForMobileTest;

    private MainLobbyPanel mainLobbyPanel;
    private StageManager stageManager;

    public static GameManager instance;

    public PlayerController playerController { get; private set; }
    public CameraController cameraController { get; private set; }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (isUnconnectedForDevelop)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("OffLineTestRoom");
        }

        cameraController = FindObjectOfType<CameraController>();
        mainLobbyPanel = GetComponentInChildren<MainLobbyPanel>();
        stageManager = GetComponentInChildren<StageManager>();
    }
    private void Start()
    {
        playerController.gameObject.SetActive(false);
    }

    public void GetUsableController(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void OnGameStartButtonClicked_MasterClient()
    {
        photonView.RPC("OnGameStartButtonClicked_All", RpcTarget.All);
    }
    [PunRPC]
    public void OnGameStartButtonClicked_All()
    {
        stageManager.OnStageStart_All();
        mainLobbyPanel.OnOffBackground(false);
    }
}
