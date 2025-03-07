using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class MainLobbyPanel : MonoBehaviourPun
{
    public Text playerCount;

    public GameObject background;
    public Button gameStartButton;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(OnGameStartClicked);
        canvasGroup = background.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
    }

    public void OnGameStartClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.OnGameStartButtonClicked_MasterClient();
        }
    }
    public void OnOffBackground(bool isOn)
    {
        if (isOn)
        {
            background.gameObject.SetActive(true);
        }
        else
        {
            background.gameObject.SetActive(false);
        }
    }
}
