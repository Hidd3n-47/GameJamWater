using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera mMainCamera;

    [SerializeField]
    private Camera mPuzzleCamera;

    private Vector3 mMainPosition;
    private Quaternion mMainRotation;
    private Vector3 mPuzzlePosition;
    private Quaternion mPuzzleRotation;

    [SerializeField]
    private float mTransitionDuration = 0.4f;

    private bool mMainCameraEnabled = true;

    private InputAction mChangeCameraActionl;

    private void Start()
    {
        mChangeCameraActionl = InputSystem.actions.FindAction("CameraChange");

        mMainCamera.gameObject.SetActive(true);
        mPuzzleCamera.gameObject.SetActive(false);

        mMainPosition = mMainCamera.gameObject.transform.position;
        mMainRotation = mMainCamera.gameObject.transform.rotation;

        mPuzzlePosition = mPuzzleCamera.gameObject.transform.position;
        mPuzzleRotation = mPuzzleCamera.gameObject.transform.rotation;
    }

    private void Update()
    {
        if(mChangeCameraActionl.WasCompletedThisFrame())
        {
            mMainCameraEnabled = !mMainCameraEnabled;
            StartCoroutine(TransitionCoroutine());
            //mMainCamera.gameObject.SetActive(mMainCameraEnabled);
            //mPuzzleCamera.gameObject.SetActive(!mMainCameraEnabled);
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        float timer = 0.0f;

        Vector3 startingPosition = mMainCameraEnabled ? mMainPosition : mPuzzlePosition;
        Vector3 endingPosition   = mMainCameraEnabled ? mPuzzlePosition : mMainPosition;

        Quaternion startingRotation = mMainCameraEnabled ? mMainRotation : mPuzzleRotation;
        Quaternion endingRotation   = mMainCameraEnabled ? mPuzzleRotation : mMainRotation;

        while (timer < mTransitionDuration)
        {
            Vector3 positionLerp = Vector3.Lerp(startingPosition, endingPosition, timer / mTransitionDuration);
            Quaternion rotationLerp = Quaternion.Lerp(startingRotation, endingRotation, timer / mTransitionDuration);

            mMainCamera.gameObject.transform.position = positionLerp;
            mMainCamera.gameObject.transform.rotation = rotationLerp;

            timer += Time.deltaTime;

            yield return null;
        }

        mMainCamera.gameObject.transform.position = endingPosition;
        mMainCamera.gameObject.transform.rotation = endingRotation;
    }
}
