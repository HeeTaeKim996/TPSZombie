using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isUnconnectedForDevelop;
    public bool isForMobileTest;


    public static GameManager instance;

    public PlayerController playerController { get; private set; }


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
    }
    private void Start()
    {
        
    }

    public void GetUsableController(PlayerController playerController)
    {
        this.playerController = playerController;
    }
}
