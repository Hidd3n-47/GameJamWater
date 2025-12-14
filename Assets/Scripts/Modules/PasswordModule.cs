using TMPro;
using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using Random = UnityEngine.Random;

public class PasswordModule : IModule
{
    public void SetPassword(string str)
    {
        mSolution = str;

        Debug.Log("Set Password to: " + str);
    }

    private string mSolution = String.Empty;
    private string mEntered  = String.Empty;

    private TextMeshProUGUI mTextDisplay;

    private void Start()
    {
        mTextDisplay = GetComponentInChildren<TextMeshProUGUI>();
        SetEnteredText(String.Empty);

        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.OnButtonPressedEvent.AddListener(OnButtonPressed);
        }

        Debug.Log("The password is: " + mSolution);
    }

    private void OnDestroy()
    {
        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.OnButtonPressedEvent.RemoveListener(OnButtonPressed);
        }
    }

    protected override void OnPassed()
    {
        SetEnteredText(String.Empty);
    }

    protected override void OnFailed()
    {
        SetEnteredText(String.Empty);
    }

    private void OnButtonPressed(int buttonId)
    {
        // Button 11 is cancel.
        if (buttonId == 11)
        {
            SetEnteredText(String.Empty);
            return;
        }

        // Button 12 is submit.
        if (buttonId == 12)
        {
            if (mEntered == mSolution)
            {
                OnPassedEventHandler?.Invoke(this, ModuleId, true);
            }
            else if (mEntered != String.Empty)
            {
                OnPassedEventHandler?.Invoke(this, ModuleId, false);
            }

            return;
        }

        // Same amount of digits entered, early exit to prevent entering a longer password.
        if (mEntered.Length == mSolution.Length)
        {
            return;
        }

        SetEnteredText(mEntered + buttonId);
    }

    private void SetEnteredText(string text)
    {
        mEntered = text;
        mTextDisplay.text = text;
    }
}
