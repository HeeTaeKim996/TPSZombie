using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageManager : MonoBehaviourPun
{
    public Transform playerSpawnTransform;

    private PlayerManager playerManager;

    public void OnStageStart_All()
    {
        GameManager.instance.playerController.gameObject.SetActive(true);
        GameObject playerObject = PhotonNetwork.Instantiate("Photon/Player", playerSpawnTransform.position, Quaternion.identity);
        GameManager.instance.cameraController.SetIsOnStage(true);
    }
   
}
