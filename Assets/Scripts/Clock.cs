using Unity.Mathematics;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] 
    private GameManager mGameManager;

    [SerializeField]
    private Transform mHourHand;
    [SerializeField]
    private Transform mMinuteHand;

    void Start()
    {
        if (mHourHand && mGameManager)
            mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, mGameManager.mStartShiftTime / 12.0f * 360.0f);
    }

    void Update()
    {
        if (!mGameManager) return;

        float time = math.lerp(mGameManager.mStartShiftTime, mGameManager.mEndShiftTime, mGameManager.mDayTimer / mGameManager.mTotalTimeForDay);

        mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, time / 12.0f * 360.0f);
        mMinuteHand.localRotation = Quaternion.Euler(0.0f, 0.0f, (time - (int)time) * 360.0f);
    }
}
