using TMPro;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PasswordModule : IModule
{
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
                OnPassedEventHandler?.Invoke();
            }
            else if (mEntered != String.Empty)
            {
                OnFailedEventHandler?.Invoke();
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
