using System;
using UnityEngine;
using Random = System.Random;

public class SpinnyModule : IModule
{
    [SerializeField]
    private float mDurationToPass = 2.0f;

    private float mTimer;

    [SerializeField]
    private Transform mNeedle;

    [SerializeField]
    private Transform mValve;

    [SerializeField]
    private Transform mSolutionBox;

    [SerializeField]
    private float mRotationSpeed = 13.0f;

    [SerializeField] 
    private float mValveSpeedMultiplier = 3.0f;
    [SerializeField]
    private float mDecayRate = 3.0f;

    [SerializeField]
    private float mMinAngle = 0.0f;

    [SerializeField]
    private float mMaxAngle = 27.0f;

    private bool mMouseDown = false;

    private bool mRotationDisabled;
    public void DisableRotation()
    {
        mRotationDisabled = true;
    }

    public void EnableRotation()
    {
        mRotationDisabled = false;
    }

    private bool mRotationDecayDisabled;
    public void DisableDecayRotation()
    {
        mRotationDecayDisabled = true;
    }

    private bool mInGoal = false;

    public void SetInGoal(bool inGoal)
    {
        mInGoal = inGoal;
    }

    public void EnableDecayRotation()
    {
        mRotationDecayDisabled = false;
    }

    private void Start()
    {
        InitPuzzle();
    }

    protected override void InitPuzzle()
    {
        mTimer = 0.0f;

        mNeedle.eulerAngles = new Vector3(0.0f, mMinAngle, 0.0f);
        mValve.eulerAngles = Vector3.zero;

        mSolutionBox.eulerAngles = new Vector3(0.0f, UnityEngine.Random.Range(0.0f, mMaxAngle), 0.0f);
    }

    private void Update()
    {
        if (mInGoal)
        {
            mTimer += Time.deltaTime;

            if (mTimer >= mDurationToPass)
            {
                OnPassedEventHandler?.Invoke(this, ModuleId, true);
                return;
            }
        }
        else
        {
            mTimer -= Time.deltaTime;

            mTimer = Math.Max(0.0f, mTimer);
        }

        ModuleButton button = GetComponentInChildren<ModuleButton>();
        if (button.Hovering && Input.GetMouseButtonDown(0))
        {
            mMouseDown = true;
        }

        if (!mMouseDown)
        {
            if (!mRotationDecayDisabled)
            {
                mNeedle.eulerAngles = new Vector3(0.0f, mNeedle.eulerAngles.y - mDecayRate * Time.deltaTime, 0.0f);
                mValve.eulerAngles = new Vector3(0.0f,
                    mValve.eulerAngles.y - mDecayRate * Time.deltaTime * mValveSpeedMultiplier, 0.0f);
            }

            return;
        }

        if (!Input.GetMouseButton(0))
        {
            mMouseDown = false;
            return;
        }

        if (mRotationDisabled)
        {
            return;
        }

        mNeedle.eulerAngles = new Vector3(0.0f, mNeedle.eulerAngles.y + mRotationSpeed * Time.deltaTime, 0.0f);
        mValve.eulerAngles = new Vector3(0.0f, mValve.eulerAngles.y + mRotationSpeed * Time.deltaTime * mValveSpeedMultiplier, 0.0f);
    }
}
