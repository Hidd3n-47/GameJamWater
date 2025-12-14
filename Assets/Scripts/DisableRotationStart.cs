using UnityEngine;

public class DisableRotationStart : MonoBehaviour
{
    private SpinnyModule mModule;

    public void Awake()
    {
        mModule = GetComponentInParent<SpinnyModule>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mModule.DisableDecayRotation();
    }
    private void OnTriggerExit(Collider other)
    {
        mModule.EnableDecayRotation();
    }
}
