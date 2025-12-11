using System;
using UnityEngine;
using UnityEngine.Events;

public class IModule : MonoBehaviour
{
    // Todo Christian: I think a good way to do this is to have an ID for the module, that way we can automatically handle the passing/failing.
    // ID will be mapped to game object on a manager?
    public UnityEvent OnPassedEventHandler;
    public UnityEvent OnFailedEventHandler;

    private void Awake()
    {
        OnPassedEventHandler.AddListener(OnPassed);
        OnPassedEventHandler.AddListener(DisableOnComplete);
        OnPassedEventHandler.AddListener(OnFailed);
    }

    private void OnDestroy()
    {
        OnPassedEventHandler.RemoveListener(OnPassed);
        OnPassedEventHandler.RemoveListener(DisableOnComplete);
        OnPassedEventHandler.RemoveListener(OnFailed);
    }

    protected virtual void OnPassed()
    {
        Debug.Log("Passed module!");
    }

    protected virtual void OnFailed()
    {
        Debug.Log("Failed module!");
    }

    protected virtual void DisableOnComplete()
    {
        gameObject.GetComponent<IModule>().enabled = false;
    }
}
