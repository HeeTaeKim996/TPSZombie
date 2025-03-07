using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.InteropServices;


public class PlayerMovement : MonoBehaviourPun
{

    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private PlayerShooter playerShooter;
    private CameraController cameraController;

    public Transform cameraNormalPivot;
    private float movementSpeed = 5f;
    private bool isMoveInvoked;
    private Vector3 moveVector;
    private Quaternion lookRotation;

    private string currentAnimation;
    private string currentUpperAnimation;
    private const string Idle = "Idle";
    private const string MoveBlend = "MoveBlend";
    private const string Aim = "Aim";
    private const string Shoot = "Shoot";


    private Coroutine setUpperLayerCoroutine;
    private float upperLayerWeight = 0;

    private bool isAimInvoked = false;
    private bool isAimOn = false;
    Vector3 aimPivotPosition;

    private Coroutine waitAndLayerDownCoroutine;
    private Coroutine upperActionCoroutine;

    private State state = State.Normal;

    private RaycastHit aimHit;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerShooter = GetComponent<PlayerShooter>();
    }
    private void Start()
    {
        lookRotation = transform.rotation;
        if (photonView.IsMine)
        {
            GameManager.instance.playerController.GetPlayerMovement(this);
            cameraController = GameManager.instance.cameraController;
            cameraController.GetNormalPivot(cameraNormalPivot);
            cameraController.GetPlayerQuaternion(lookRotation);
        }
    }
    private void Update()
    {

        if (photonView.IsMine && !isMoveInvoked && (state == State.Normal || state == State.UpperAction))
        {
            BaseAnimationCross(Idle, 0.05f);
        }

        if(photonView.IsMine && state != State.UpperAction)
        {
            UpperAnimationCross(Aim, 0.05f);
        }

        if (isAimOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray.origin, ray.direction, out aimHit, 100f))
            {
                aimPivotPosition = aimHit.point;
            }
            else
            {
                aimPivotPosition = cameraNormalPivot.position + lookRotation * Vector3.forward * 5f;
            }
        }
    }
    private void FixedUpdate()
    {
        if (photonView.IsMine && isMoveInvoked && (state == State.Normal || state == State.UpperAction))
        {
            playerRigidbody.MovePosition(playerRigidbody.position + moveVector * movementSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        playerAnimator.SetLookAtWeight(upperLayerWeight, upperLayerWeight, upperLayerWeight, 0.05f, 0.05f);
        playerAnimator.SetLookAtPosition(aimPivotPosition);
    }
    private IEnumerator SetUpperLayerCoroutine(bool isUp)
    {
        if (isUp)
        {
            while (upperLayerWeight < 1)
            {
                upperLayerWeight += Time.deltaTime * (1f / 0.05f);
                playerAnimator.SetLayerWeight(1, upperLayerWeight);

                yield return null;
            }
            upperLayerWeight = 1;
            playerAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            while (upperLayerWeight > 0)
            {
                upperLayerWeight -= Time.deltaTime * (1f / 0.2f);
                playerAnimator.SetLayerWeight(1, upperLayerWeight);

                yield return null;
            }
            upperLayerWeight = 0;
            playerAnimator.SetLayerWeight(1, 0);
        }

        setUpperLayerCoroutine = null;
    }



    #region Controller Input
    public void OnMoveTouchBegan()
    {
        Debug.Log("Began Check");
        isMoveInvoked = true;
    }
    public void OnMoveTouchDrag(Vector2 vector2)
    {
        float theta = Mathf.Atan2(vector2.y, vector2.x);
        //moveVector = Quaternion.Euler(0, -theta * Mathf.Rad2Deg, 0) * transform.right * vector2.magnitude;
        // -theta인 이유는 요우 회전의 수평축은 Z, 수직축은 X이므로, theta 가 아닌 -theta 적용 필요
        // 이 코드가 코드 설명이 직관적이기에 위 코드를 사용하고 싶지만, Quaternion.AngleAxis 가 한 축을 기준으로만 회전하기 때문에, 한축만 회전할 시 연산 효율이 더 좋다 한다(지피티왈). 그래서 Quaternion.AngleAxis 코드 사용

        moveVector = Quaternion.AngleAxis(-theta * Mathf.Rad2Deg, Vector3.up) * transform.right * vector2.magnitude;
        // 위 주석과 마찬가지로, Vector3.up 인 요우 회전의 수평축은 Z, 수직축은 X이기 때문에, -theta 회전


        if (true) // 추후 조건 추가
        {
            float blendTime = currentAnimation == Idle ? 0.03f : 0.05f;

            BaseAnimationCross(MoveBlend, blendTime);
            playerAnimator.SetFloat("Horizontal", vector2.x);
            playerAnimator.SetFloat("Vertical", vector2.y);
            playerAnimator.SetFloat("Magnitude", vector2.magnitude);
        }
    }
    public void OnMoveTOuchEnd()
    {
        Debug.Log("End Check");
        isMoveInvoked = false;
    }
    public void OnRotTouchBegan()
    {

    }
    public void OnRotTouchDrag(Vector2 vector2)
    {
        float buffer = 0.5f;

        float vectorX = vector2.x * buffer;
        float vectorY = vector2.y * buffer;

        float eulerAnglesX = lookRotation.eulerAngles.x;
        Quaternion rotation;
        if ((eulerAnglesX >= 0 && eulerAnglesX <= 60) || (eulerAnglesX <= 360 && eulerAnglesX >= 320))
        {
            rotation = lookRotation * Quaternion.Euler(-vectorY, vectorX, 0);
        }
        else
        {
            rotation = lookRotation * Quaternion.Euler(0, vectorX, 0);
        }

        eulerAnglesX = rotation.eulerAngles.x;

        if (eulerAnglesX > 180 && eulerAnglesX <= 320)
        {
            eulerAnglesX = 320;
        }
        else if (eulerAnglesX < 180 && eulerAnglesX >= 60)
        {
            eulerAnglesX = 60;
        }

        lookRotation = Quaternion.Euler(new Vector3(eulerAnglesX, rotation.eulerAngles.y, 0));
        cameraController.GetPlayerQuaternion(lookRotation);
        transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
    }
    public void OnRotTouchEnd()
    {

    }
    public void OnFireTouchBegan()
    {
        isAimInvoked = true;
        isAimOn = true;
        if (setUpperLayerCoroutine != null)
        {
            StopCoroutine(setUpperLayerCoroutine);
        }
        setUpperLayerCoroutine = StartCoroutine(SetUpperLayerCoroutine(true));
        if (waitAndLayerDownCoroutine != null)
        {
            StopCoroutine(waitAndLayerDownCoroutine);
            waitAndLayerDownCoroutine = null;
        }
    }
    public void OnFireTouchDrag()
    {

    }
    public void OnFireTouchEnd()
    {
        isAimInvoked = false;

        if(state == State.Normal && playerShooter.IsShootable())
        {
            playerShooter.InstanceShoot(aimHit);

            if (upperActionCoroutine != null)
            {
                StopCoroutine(upperActionCoroutine);
            }
            upperActionCoroutine = StartCoroutine(UpperActionCoroutine(Shoot, 0.267f));
        }
        else
        {
            Debug.Log("슈팅못함 관련 UI 필요");
        }
    }
    #endregion
    private IEnumerator WaitAndLayerDownCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (!isAimInvoked)
        {
            isAimOn = false;
            if (setUpperLayerCoroutine != null)
            {
                StopCoroutine(setUpperLayerCoroutine);
            }
            setUpperLayerCoroutine = StartCoroutine(SetUpperLayerCoroutine(false));
        }
    }

    private IEnumerator UpperActionCoroutine(string animationName, float animationDuration)
    {
        state = State.UpperAction;
        UpperAnimationCoroutine(animationName, 0.05f);
        yield return new WaitForSeconds(animationDuration);


        state = State.Normal;
        UpperAnimationCross(Aim, 0.05f);
        if (waitAndLayerDownCoroutine != null)
        {
            StopCoroutine(waitAndLayerDownCoroutine);
        }
        waitAndLayerDownCoroutine = StartCoroutine(WaitAndLayerDownCoroutine());

        upperActionCoroutine = null;
    }

    #region PrimaryMethods
    private void BaseAnimationCross(string animationName, float blendTime, float normalizedTime = 0)
    {
        if (currentAnimation == animationName) return;

        playerAnimator.CrossFade(animationName, blendTime, 0, normalizedTime);
        currentAnimation = animationName;
    }
    private void UpperAnimationCross(string animationName, float blendTime, float normalizedTime = 0)
    {
        if (currentUpperAnimation == animationName) return;
        playerAnimator.CrossFade(animationName, blendTime, 1, normalizedTime);
        currentUpperAnimation = animationName;
    }
    private void UpperAnimationCoroutine(string animationName, float blendTime)
    {
        playerAnimator.CrossFade(animationName, blendTime, 1, 0);
        currentUpperAnimation = animationName;
    }
    #endregion
}
