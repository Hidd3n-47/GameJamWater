using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera mMainCamera;

    [SerializeField]
    private Camera mPuzzleCamera;

    [SerializeField]
    private Light mMainLight;

    [SerializeField]
    private Light mPuzzleLight;

    [SerializeField]
    private float mDelayToTurnMainCameraLightOn = 0.2f;

    private Vector3    mMainPosition;
    private Quaternion mMainRotation;
    private Vector3    mPuzzlePosition;
    private Quaternion mPuzzleRotation;

    private float mMainCameraSize;
    private float mPuzzleCameraSize;

    [SerializeField]
    private float mTransitionDuration = 0.4f;

    private bool mMainCameraEnabled = true;

    private InputAction mChangeCameraAction;

    public bool MainCameraView => mMainCameraEnabled;

    private void Start()
    {
        mChangeCameraAction = InputSystem.actions.FindAction("CameraChange");

        mMainCamera.gameObject.SetActive(true);
        mPuzzleCamera.gameObject.SetActive(false);

        mMainPosition = mMainCamera.gameObject.transform.position;
        mMainRotation = mMainCamera.gameObject.transform.rotation;

        mPuzzlePosition = mPuzzleCamera.gameObject.transform.position;
        mPuzzleRotation = mPuzzleCamera.gameObject.transform.rotation;

        mMainCameraSize   = mMainCamera.orthographicSize;
        mPuzzleCameraSize = mPuzzleCamera.orthographicSize;
    }

    private void Update()
    {
        if(mChangeCameraAction.WasCompletedThisFrame())
        {
            Trans();
        }
    }

    public void Trans()
    {
        StartCoroutine(TransitionCoroutine());
        if (mMainCameraEnabled)
        {
            mMainLight.gameObject.SetActive(false);
            mPuzzleLight.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(EnableMainCamera());
        }

        mMainCameraEnabled = !mMainCameraEnabled;
    }

    private IEnumerator TransitionCoroutine()
    {
        float timer = 0.0f;

        Vector3 startingPosition = mMainCameraEnabled ? mMainPosition : mPuzzlePosition;
        Vector3 endingPosition   = mMainCameraEnabled ? mPuzzlePosition : mMainPosition;

        Quaternion startingRotation = mMainCameraEnabled ? mMainRotation : mPuzzleRotation;
        Quaternion endingRotation   = mMainCameraEnabled ? mPuzzleRotation : mMainRotation;

        float startingSize = mMainCameraEnabled ? mMainCameraSize : mPuzzleCameraSize;
        float endingSize   = mMainCameraEnabled ? mPuzzleCameraSize : mMainCameraSize;

        while (timer < mTransitionDuration)
        {
            Vector3 positionLerp = Vector3.Lerp(startingPosition, endingPosition, timer / mTransitionDuration);
            Quaternion rotationLerp = Quaternion.Lerp(startingRotation, endingRotation, timer / mTransitionDuration);
            float sizeLerp = math.lerp(startingSize, endingSize, timer / mTransitionDuration);

            mMainCamera.gameObject.transform.position = positionLerp;
            mMainCamera.gameObject.transform.rotation = rotationLerp;
            mMainCamera.orthographicSize = sizeLerp;

            timer += Time.deltaTime;

            yield return null;
        }

        mMainCamera.gameObject.transform.position = endingPosition;
        mMainCamera.gameObject.transform.rotation = endingRotation;
        mMainCamera.orthographicSize = endingSize;
    }

    private IEnumerator EnableMainCamera()
    {
        float timer = 0.0f;

        while (timer < mDelayToTurnMainCameraLightOn)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        mMainLight.gameObject.SetActive(true);
        mPuzzleLight.gameObject.SetActive(false);
    }
}
