using Unity.Mathematics;
using UnityEngine;

public class mainMenuClock : MonoBehaviour
{
    [SerializeField]
    private Transform mHourHand;
    [SerializeField]
    private Transform mMinuteHand;

    public float speed;

    private float mTimer;

    void Start()
    {
        mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, 9.0f / 12.0f * 360.0f);
    }

    void Update()
    {
        mTimer += Time.deltaTime * speed;

        if (mTimer >= 24.0f)
        {
            mTimer = 0.0f;
        }

        mHourHand.localRotation = Quaternion.Euler(0.0f, 0.0f, mTimer / 12.0f * 360.0f);
        mMinuteHand.localRotation = Quaternion.Euler(0.0f, 0.0f, (mTimer - (int)mTimer) * 360.0f);
    }
}
