using System;
using UnityEngine;

public class IModule : MonoBehaviour
{
    // Todo Christian: I think a good way to do this is to have an ID for the module, that way we can automatically handle the passing/failing.
    // ID will be mapped to game object on a manager?
    public EventHandler OnPassedEventHandler;
    public EventHandler OnFailedEventHandler;

    private void Awake()
    {
        OnPassedEventHandler += OnPassed;
        OnPassedEventHandler += DisableOnComplete;
        OnFailedEventHandler += OnFailed;
    }

    private void OnDestroy()
    {
        OnFailedEventHandler -= OnFailed;
        OnPassedEventHandler -= DisableOnComplete;
        OnPassedEventHandler -= OnPassed;
    }

    protected virtual void OnPassed(object sender, EventArgs args)
    {
        Debug.Log("Passed module!");
    }

    protected virtual void OnFailed(object sender, EventArgs args)
    {
        Debug.Log("Failed module!");
    }
    protected virtual void DisableOnComplete(object sender, EventArgs args)
    {
        gameObject.GetComponent<IModule>().enabled = false;
    }
}
