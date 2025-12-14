using System;
using UnityEngine;

public class DisableRotation : MonoBehaviour
{
    private SpinnyModule mModule;

    public void Awake()
    {
        mModule = GetComponentInParent<SpinnyModule>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mModule.DisableRotation();
    }
    private void OnTriggerExit(Collider other)
    {
        mModule.EnableRotation();
    }
}
