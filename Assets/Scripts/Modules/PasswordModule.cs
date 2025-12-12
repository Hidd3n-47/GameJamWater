using TMPro;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class PasswordModule : IModule
{
    private string mSolution = String.Empty;
    private string mEntered  = String.Empty;

    private TextMeshProUGUI mTextDisplay;

    [Header("Light")]
    [SerializeField]
    private Transform mLightTransform;
    [SerializeField]
    private Material mPassedMaterial;
    [SerializeField]
    private Material mFailedMaterial;

    private void Start()
    {
        mTextDisplay = GetComponentInChildren<TextMeshProUGUI>();
        SetEnteredText(String.Empty);

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

    protected override void OnPassed(object sender, EventArgs args)
    {
        //mLightTransform.GetComponent<MeshRenderer>().material = mPassedMaterial;
        SetEnteredText(String.Empty);
    }

    protected override void DisableOnComplete(object sender, EventArgs args)
    {
        foreach (ModuleButton button in GetComponentsInChildren<ModuleButton>())
        {
            button.enabled = false;
        }

        base.DisableOnComplete(sender, args);
    }

    protected override void OnFailed(object sender, EventArgs args)
    {
        //mLightTransform.GetComponent<MeshRenderer>().material = mFailedMaterial;
        SetEnteredText(String.Empty);
    }

    private void OnButtonPressed(object sender, int buttonId)
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
                OnPassedEventHandler?.Invoke(this, EventArgs.Empty);
                Debug.Log("Passed Password");
            }
            else
            {
                OnFailedEventHandler?.Invoke(this, EventArgs.Empty);
                Debug.Log("Failed Password");
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
