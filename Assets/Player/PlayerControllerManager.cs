using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    private PlayerController usableController;
    private PlayerController_Mobile playerController_Mobile;
    private PlayerController_PC playerController_PC;

    private void Awake()
    {
        playerController_Mobile = GetComponentInChildren<PlayerController_Mobile>();
        playerController_PC = GetComponentInChildren<PlayerController_PC>();
        AssignController();
    }


    private void AssignController()
    {

        if (GameManager.instance.isForMobileTest)
        {
            playerController_PC.gameObject.SetActive(false);
            usableController = playerController_Mobile;
        }
        else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            playerController_PC.gameObject.SetActive(false);
            usableController = playerController_Mobile;
        }
        else
        {
            playerController_Mobile.gameObject.SetActive(false);
            usableController = playerController_PC;
        }


        GameManager.instance.GetUsableController(usableController);
    }


}
