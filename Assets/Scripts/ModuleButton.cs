using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModuleButton : MonoBehaviour
{
    public EventHandler<int> OnButtonPressedEvent;

    [SerializeField]
    private int mButtonId;

    Camera mCamera;

    private InputAction mMouseAction;
    private InputAction mButtonAction;

    private void Awake()
    {
        mMouseAction  = InputSystem.actions.FindAction("MousePosition");
        mButtonAction = InputSystem.actions.FindAction("MouseDown");

        //mCamera = GameObject.Find("Puzzle Camera").GetComponent<Camera>();
        mCamera = Camera.main;
    }

    public void Update()
    {
        if (!mButtonAction.WasCompletedThisFrame())
        {
            return;
        }

        Ray ray = mCamera.ScreenPointToRay(mMouseAction.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject == gameObject)
            {
                OnButtonPressedEvent?.Invoke(this, mButtonId);
            }
        }
    }
}
