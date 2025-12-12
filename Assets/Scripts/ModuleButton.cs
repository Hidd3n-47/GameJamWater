using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class ModuleButton : MonoBehaviour
{
    public UnityEvent<int> OnButtonPressedEvent;

    [SerializeField]
    private int mButtonId;

    Camera mCamera;

    private InputAction mMouseAction;
    private InputAction mButtonAction;

    private void Awake()
    {
        mMouseAction  = InputSystem.actions.FindAction("MousePosition");
        mButtonAction = InputSystem.actions.FindAction("MouseDown");

        mCamera = Camera.main;
    }

    public void Update()
    {
        bool hovered = false;
        Ray ray = mCamera.ScreenPointToRay(mMouseAction.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject == gameObject)
            {
                hovered = true;
            }
        }

        if (hovered)
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

        OnButtonPressedEvent?.Invoke(mButtonId);
    }
}
