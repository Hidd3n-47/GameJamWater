using System;
using UnityEngine;

public class PasswordModule : IModule
{
    // Todo Christian: Look at making this an int instead of a string.
    private string mSolution = "1234";
    private string mEntered  = String.Empty;

    private void Start()
    {
        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.OnButtonPressedEvent += OnButtonPressed;
        }
    }

    private void OnDestroy()
    {
        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.OnButtonPressedEvent -= OnButtonPressed;
        }
    }

    private void OnButtonPressed(object sender, int buttonId)
    {
        mEntered += buttonId;
        Debug.Log("Button pressed: " + buttonId + " - entered: " + mEntered);

        if (mEntered.Length < mSolution.Length)
        {
            return;
        }

        // Todo Christian: This shouldn't auto submit, should only submit when pressing the confirm button.
        if (mEntered == mSolution)
        {
            OnPassed?.Invoke(this, EventArgs.Empty);
            Debug.Log("Passed Password");
        }
        else
        {
            OnFailed?.Invoke(this, EventArgs.Empty);
            Debug.Log("Failed Password");
        }

        mEntered = String.Empty;
    }
}
