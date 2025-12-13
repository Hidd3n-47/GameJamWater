using System;
using System.Linq;
using UnityEngine;

public class LevelsModule : IModule
{
    [SerializeField]
    private Transform mLeftSide;
    [SerializeField]
    private Transform mRightSide;

    private Transform[] mLeftSideNodes;
    private Transform[] mRightSideNodes;

    [SerializeField]
    private float mTotalHoldDuration;

    private int mNumberOfPipes;

    private int mNumberOfPipesLit;

    private float mHoldTime;

    [SerializeField]
    private AudioSource mSource;
    private float mStartingPitch;

    [SerializeField]
    private float mPitchIncreasePerLight = 0.01f;

    private void Start()
    {
        mRightSideNodes = mRightSide.GetComponentsInChildren<Transform>().Where(x => x != mRightSide).ToArray();
        mLeftSideNodes  = mLeftSide.GetComponentsInChildren<Transform>().Where(x => x != mLeftSide).ToArray();

        mNumberOfPipes = mRightSideNodes.Length;

        foreach (var t in mRightSideNodes)
        {
            t.gameObject.SetActive(false);
        }

        mStartingPitch = mSource.pitch;
    }

    private void Update()
    {
        if (mNumberOfPipesLit >= mNumberOfPipes) return;

        ModuleButton button = GetComponentInChildren<ModuleButton>();
        if (!(button.Hovering && button.MouseDown))
        {
            if (mHoldTime <= 0.0f)
            {
                mHoldTime = 0.0f;
                return;
            }

            mHoldTime -= Time.deltaTime;

            float timer = (mNumberOfPipesLit) * mTotalHoldDuration / mNumberOfPipes;

            if (mHoldTime < timer)
            {
                mLeftSideNodes[mNumberOfPipes - mNumberOfPipesLit - 1].gameObject.SetActive(true);
                mRightSideNodes[mNumberOfPipesLit].gameObject.SetActive(false);

                if (mNumberOfPipesLit > 0)
                {
                    --mNumberOfPipesLit;
                   PlaySound();
                }
            }

            return;
        }

        mHoldTime += Time.deltaTime;

        float timeForPreviousRemoval = (mNumberOfPipesLit + 1) * mTotalHoldDuration / mNumberOfPipes;

        if (mHoldTime > timeForPreviousRemoval)
        {
            mLeftSideNodes[mNumberOfPipes - mNumberOfPipesLit - 1].gameObject.SetActive(false);
            mRightSideNodes[mNumberOfPipesLit].gameObject.SetActive(true);

            ++mNumberOfPipesLit;
            PlaySound();
        }

        if (mNumberOfPipesLit >= mNumberOfPipes)
        {
            OnPassedEventHandler?.Invoke();
        }
    }

    private void PlaySound()
    {
        mSource.pitch = mStartingPitch + mPitchIncreasePerLight * mNumberOfPipesLit;
        mSource.Play();
    }
}
