using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform normalPivot;
    Quaternion lookRotation;
    private bool isOnStage;

    public void GetNormalPivot(Transform normalPivot)
    {
        this.normalPivot = normalPivot;
    }

    public void GetPlayerQuaternion(Quaternion lookRotation)
    {
        this.lookRotation = lookRotation;
        transform.position = normalPivot.position - transform.forward * 2f;
    }

    public void SetIsOnStage(bool isOnStage)
    {
        this.isOnStage = isOnStage;
    }

    private void LateUpdate()
    {
        if (!isOnStage) return;

        transform.rotation = lookRotation;
        transform.position = normalPivot.position - transform.forward * 2f;
    }
}
