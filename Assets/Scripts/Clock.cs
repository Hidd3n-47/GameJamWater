using Unity.Mathematics;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] 
    private GameManager mGameManager;

    [SerializeField]
    private Transform mMinuteHand;

    void Update()
    {

        float time = math.lerp(mGameManager.mStartShiftTime, mGameManager.mEndShiftTime, mGameManager.mDayTimer / mGameManager.mTotalTimeForDay);

        int minutes = (int)((time - (int)time) * 60.0f);

        mMinuteHand.eulerAngles = new Vector3(0.0f, 0.0f, minutes / 60.0f);

        //mClockTimer.text = ((int)time) + ":" + (minutes < 10 ? "0" + minutes : minutes);
    }
}
