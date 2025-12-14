using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline), typeof(BoxCollider))]
public class ModuleButton : MonoBehaviour
{
    public UnityEvent<int> OnButtonPressedEvent;

    [SerializeField]
    private int mButtonId;

    private CameraManager mCameraManager;

    private InputAction mMouseAction;
    private InputAction mButtonAction;

    private bool mHovering;
    private bool mMouseDown;

    public bool Hovering => mHovering;
    public bool MouseDown => mMouseDown;

    public void Reset()
    {
        mMouseDown = false;
    }

    private void Start()
    {
        mMouseAction  = InputSystem.actions.FindAction("MousePosition");
        mButtonAction = InputSystem.actions.FindAction("MouseDown");

        mCameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    public void Update()
    {
        if (mCameraManager.MainCameraView)
        {
            GetComponent<Outline>().enabled = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mMouseDown = false;
        }


        mHovering = false;
        Ray ray = Camera.main.ScreenPointToRay(mMouseAction.ReadValue<Vector2>());

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
            return;
        }

        mMouseDown = false;

        OnButtonPressedEvent?.Invoke(mButtonId);
    }
}
