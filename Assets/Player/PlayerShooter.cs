using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShooter : MonoBehaviourPun
{
    private PlayerManager playerManager;
    private PlayerMovement playerMovement;
    public Transform firePivot;
    private LayerMask attackableLayer;


    private int ammo;
    private int magMaxAmmo;
    private int magAmmo;

    private float damage;
    


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        attackableLayer = CommonMethods.StringsToLayerMask(playerManager.stringsOfAttackable);
    }

    public bool IsShootable()
    {
        return magAmmo > 0;
    }


    public void InstanceShoot(RaycastHit hit)
    {
        if( ( (1 << hit.collider.gameObject.layer) & attackableLayer) != 0)
        {
            Debug.Log("Hittable Check");
        }
    }
}
