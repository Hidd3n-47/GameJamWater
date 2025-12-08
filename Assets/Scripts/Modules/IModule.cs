using System;
using UnityEngine;

public class IModule : MonoBehaviour
{
    // Todo Christian: I think a good way to do this is to have an ID for the module, that way we can automatically handle the passing/failing.
    // ID will be mapped to game object on a manager?
    public EventHandler OnPassed;
    public EventHandler OnFailed;
}
