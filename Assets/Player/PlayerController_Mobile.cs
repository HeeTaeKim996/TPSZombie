using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Mobile : PlayerController
{

    public RectTransform moveRect;
    private float moveRadius;
    private int moveId = -1;
    private bool didTouchMove = false;
    private Vector2 moveInitPosition;
    private Vector2 moveStartPosition;

    private int rotId = -1;
    private bool didTouchRotate = false;
    private Vector2 preRotVec;

    public RectTransform fireRect;
    private float fireRadius;
    private int fireId = -1;
    private bool didTouchFire = false;


    private void Awake()
    {
        
    }
    private void Start()
    {
        moveInitPosition = moveRect.anchoredPosition;
        moveRadius = moveRect.rect.width / 2f * 0.9f;
        fireRadius = fireRect.rect.width / 2f * 0.9f;
    }

    private void Update()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (moveId == -1 || moveId == touch.fingerId)
            {
                if(touch.phase == TouchPhase.Began && IsTouchWithinCircle(touch.position, moveRect, moveRadius))
                {
                    moveStartPosition = touch.position;
                    moveRect.anchoredPosition = moveStartPosition;
                    playerMovement.OnMoveTouchBegan();

                    moveId = touch.fingerId;
                    didTouchMove = true;

                    Debug.Log(touch.position);
                }
                else if( ( touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && didTouchMove)
                { 
                    Vector2 vector = touch.position - moveStartPosition;
                    Vector2 refinedVector = vector.magnitude > moveRadius ? vector.normalized : vector / moveRadius;
                    playerMovement.OnMoveTouchDrag(refinedVector);
                }
                else if( ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && didTouchMove)
                {
                    moveRect.anchoredPosition = moveInitPosition;
                    playerMovement.OnMoveTOuchEnd();

                    moveId = -1;
                    didTouchMove = false;
                }
            }

            if(rotId == -1 || rotId == touch.fingerId)
            {
                if(touch.phase == TouchPhase.Began && (!IsTouchWithinCircle(touch.position, moveRect, moveRadius)))
                {
                    playerMovement.OnRotTouchBegan();
                    preRotVec = touch.position;
                    rotId = touch.fingerId;
                    didTouchRotate = true;
                }
                else if( (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && didTouchRotate)
                {
                    playerMovement.OnRotTouchDrag(touch.position - preRotVec);
                    preRotVec = touch.position;
                }
                else if( (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && didTouchRotate)
                {
                    playerMovement.OnRotTouchEnd();
                    rotId = -1;
                    didTouchRotate = false;
                }
            }

            if (fireId == -1 || fireId == touch.fingerId)
            {
                if(touch.phase == TouchPhase.Began && IsTouchWithinCircle(touch.position, fireRect, fireRadius))
                {
                    playerMovement.OnFireTouchBegan();

                    fireId = touch.fingerId;
                    didTouchFire = true;
                }
                else if( (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && didTouchFire)
                {
                    playerMovement.OnFireTouchDrag();
                }
                else if( (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && didTouchFire)
                {
                    playerMovement.OnFireTouchEnd();

                    fireId = touch.fingerId;
                    didTouchFire = false;
                }

            }
        }
    }






    private bool IstouchWithinRect(Vector2 touchPosition, RectTransform rectTransform)
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touchPosition, null, out localPoint))
        {
            return rectTransform.rect.Contains(localPoint);
        }
        return false;
    }
    private bool IsTouchWithinCircle(Vector2 touchPosition, RectTransform rectTransform, float radius)
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touchPosition, null, out localPoint))
        {
            return localPoint.magnitude <= radius;
        }
        return false;
    }
}
