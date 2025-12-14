using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pipe : MonoBehaviour
{
    private readonly float[] mRotationAngles = { 0.0f, 90.0f, 180.0f, 270.0f };

    private ConnectPipeModule mModule   = null;
    private int   mRotationIndex        = 0;
    private bool  mCornderPiece         = false;
    private float mCorrectAngle         = 0.0f;
    private bool  mInCorrectOrientation = false;

    public void Init(ConnectPipeModule module, float correctAngle, bool cornerPiece)
    {
        mModule = module;
        mCorrectAngle = correctAngle;
        mCornderPiece = cornerPiece;

        mRotationIndex = Random.Range(0, mRotationAngles.Length);

        gameObject.transform.localRotation = Quaternion.Euler(0, mRotationAngles[mRotationIndex], 0);

        mInCorrectOrientation = IsCorrectOrientation();
        if (IsCorrectOrientation())
        {
            mModule.PipeInCorrectPosition();
        }

        GetComponentInChildren<ModuleButton>().OnButtonPressedEvent.AddListener(OnClicked);
    }

    private void OnDestroy()
    {
        GetComponentInChildren<ModuleButton>().OnButtonPressedEvent.RemoveListener(OnClicked);
    }

    public void OnClicked(int buttonId)
    {
        if (!mModule.isActiveAndEnabled)
        {
            return;
        }

        mRotationIndex = (mRotationIndex + 1) % mRotationAngles.Length;
        gameObject.transform.localRotation = Quaternion.Euler(0, mRotationAngles[mRotationIndex], 0);

        bool wasInCorrectOrientation = mInCorrectOrientation;
        mInCorrectOrientation = IsCorrectOrientation();

        if (IsCorrectOrientation())
        {
            mModule.PipeInCorrectPosition();
        }
        else if (wasInCorrectOrientation)
        {
            mModule.PipeRemovedFromCorrectPosition();
        }
    }

    bool IsCorrectOrientation()
    {
        float angle = gameObject.transform.localEulerAngles.y;

        if (mCornderPiece) return angle.Equals(mCorrectAngle);

        return angle.Equals(mCorrectAngle) || Math.Abs(angle - mCorrectAngle).Equals(180.0f);
    }
}
