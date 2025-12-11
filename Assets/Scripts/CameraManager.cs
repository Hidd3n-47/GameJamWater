using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera mMainCamera;

    [SerializeField]
    private Camera mPuzzleCamera;

    [SerializeField]
    private float mTransitionDuration;

    private bool mMainCameraEnabled = true;

    private InputAction mChangeCameraActionl;

    private void Start()
    {
        mChangeCameraActionl = InputSystem.actions.FindAction("CameraChange");

        mMainCamera.gameObject.SetActive(true);
        mPuzzleCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(mChangeCameraActionl.WasCompletedThisFrame())
        {
            mMainCameraEnabled = !mMainCameraEnabled;

            mMainCamera.gameObject.SetActive(mMainCameraEnabled);
            mPuzzleCamera.gameObject.SetActive(!mMainCameraEnabled);
        }
    }

    //private Coroutine TransitionCoroutine()
    //{

    //}
}
