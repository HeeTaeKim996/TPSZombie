using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.InteropServices;


public class PlayerMovement : MonoBehaviourPun
{
    private Rigidbody playerRigidbody;

    private float movementSpeed = 5f;
    private bool isMoveOn;
    private Vector3 moveVector;
    private Quaternion lookRotation;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.playerController.GetPlayerMovement(this);
        }
        lookRotation = transform.rotation;
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (isMoveOn)
        {
            Debug.Log("FixedUpdate Check");
            playerRigidbody.MovePosition(playerRigidbody.position + moveVector * movementSpeed * Time.fixedDeltaTime );
        }
    }





    public void OnMoveTouchBegan()
    {
        Debug.Log("Began Check");
        isMoveOn = true;
    }
    public void OnMoveTouchDrag(Vector2 vector2)
    {
        moveVector = new Vector3(vector2.x, 0, vector2.y);
    }
    public void OnMoveTOuchEnd()
    {
        Debug.Log("End Check");
        isMoveOn = false;
    }
    public void OnRotTouchBegan()
    {

    }
    public void OnRotTouchDrag(Vector2 vector2)
    {
        float eulerAnglesX = lookRotation.eulerAngles.x;
        Quaternion rotation;
        if( (eulerAnglesX >= 0 && eulerAnglesX <= 60 ) || ( eulerAnglesX <= 360 && eulerAnglesX >= 320))
        {
            rotation = lookRotation * Quaternion.Euler(-vector2.y, vector2.x, 0);
        }
        else
        {
            rotation = lookRotation * Quaternion.Euler(0, vector2.x, 0);
        }

        eulerAnglesX = rotation.eulerAngles.x;

        if(eulerAnglesX > 180 && eulerAnglesX <= 320)
        {
            eulerAnglesX = 320;
        }
        else if(eulerAnglesX < 180 && eulerAnglesX >= 60)
        {
            eulerAnglesX = 60;
        }

        lookRotation = Quaternion.Euler(new Vector3(eulerAnglesX, rotation.eulerAngles.y, 0));
        transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
    }
    public void OnRotTouchEnd()
    {

    }

}
