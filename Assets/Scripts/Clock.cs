using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    [SerializeField] 
    private GameManager mGameManager;

    [SerializeField]
    private Transform mHourHand;
    [SerializeField]
    private Transform mMinuteHand;

    private bool mClockingOutAlready = false;


    void Start()
    {
        if (mHourHand && mGameManager)
            mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, mGameManager.mStartShiftTime / 12.0f * 360.0f);
    }

    void Update()
    {
        if (!mGameManager) return;

        float time = math.lerp(mGameManager.mStartShiftTime, mGameManager.mEndShiftTime, mGameManager.mDayTimer / mGameManager.mTotalTimeForDay);

        if (time >= mGameManager.mEndShiftTime && !mClockingOutAlready)
        {
            mClockingOutAlready = !mClockingOutAlready;

            StartCoroutine(Clockout());
        }

        mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, time / 12.0f * 360.0f);
        mMinuteHand.localRotation = Quaternion.Euler(0.0f, 0.0f, (time - (int)time) * 360.0f);
    }

    IEnumerator Clockout()
    {
        Time.timeScale = 0.0f;
        float timer = 0.0f;

        var a = GetComponent<AudioSource>();
            a.Play();
        while (timer < a.clip.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync(mGameManager.NextScene);
    }
}
