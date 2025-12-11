using UnityEngine;
using UnityEngine.InputSystem;

public class RemovePrompt : MonoBehaviour
{
    private InputAction mChangeCameraAction;

    private void Start()
    {
        mChangeCameraAction = InputSystem.actions.FindAction("CameraChange");
    }
    private void Update()
    {
        if (mChangeCameraAction.WasCompletedThisFrame())
        {
            Destroy(gameObject);
        }
    }
}
