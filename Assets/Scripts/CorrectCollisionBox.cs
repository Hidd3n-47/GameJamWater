using System;
using UnityEngine;

public class CorrectCollisionBox : MonoBehaviour
{
    private SpinnyModule mModule;

    public void Awake()
    {
        mModule = GetComponentInParent<SpinnyModule>();
    }

    private void OnTriggerEnter(Collider other)
    {
        mModule.SetInGoal(true);
    }
    private void OnTriggerExit(Collider other)
    {
        mModule.SetInGoal(false);
    }
}
