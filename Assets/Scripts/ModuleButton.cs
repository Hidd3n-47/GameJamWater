using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline), typeof(BoxCollider))]
public class ModuleButton : MonoBehaviour
{
    public UnityEvent<int> OnButtonPressedEvent;

    [SerializeField]
    private int mButtonId;

    Camera mCamera;
    private CameraManager mCameraManager;

    private InputAction mMouseAction;
    private InputAction mButtonAction;

    private bool mHovering;
    private bool mMouseDown;

    public bool Hovering => mHovering;
    public bool MouseDown => mMouseDown;

    private void Awake()
    {
        mMouseAction  = InputSystem.actions.FindAction("MousePosition");
        mButtonAction = InputSystem.actions.FindAction("MouseDown");

        mCamera = Camera.main;

        mCameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    public void Update()
    {
        if (mCameraManager.MainCameraView)
        {
            GetComponent<Outline>().enabled = false;
            return;
        }

        mHovering = false;
        Ray ray = mCamera.ScreenPointToRay(mMouseAction.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject == gameObject)
            {
                mHovering = true;
            }
        }

        if (mHovering)
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
            return;
        }

        if (!mButtonAction.WasCompletedThisFrame())
        {
            if (mButtonAction.IsInProgress())
            {
                mMouseDown = true;
            }
            return;
        }

        mMouseDown = false;

        OnButtonPressedEvent?.Invoke(mButtonId);
    }
}
