using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PasswordModule : IModule
{
    private string mSolution = String.Empty;
    private string mEntered  = String.Empty;

    private void Start()
    {
        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.OnButtonPressedEvent += OnButtonPressed;
        }

        for (int i = 0; i < 4; ++i)
        {
            mSolution += Random.Range(0, 9);
        }

        Debug.Log("The password is: " + mSolution);
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
        Debug.Log("Button pressed: " + buttonId + " - entered: " + mEntered);

        // Button 11 is cancel.
        if (buttonId == 11)
        {
            mEntered = String.Empty;
            return;
        }

        // Button 12 is submit.
        if (buttonId == 12)
        {
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
        }

        // Same amount of digits entered, early exit to prevent entering a longer password.
        if (mEntered.Length == mSolution.Length)
        {
            return;
        }

        mEntered += buttonId;
    }
}
