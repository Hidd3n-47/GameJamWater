using UnityEngine;

public class LevelsModule : MonoBehaviour
{
    [SerializeField]
    private Transform mLeftSide;
    [SerializeField]
    private Transform mRightSide;

    [SerializeField]
    private float mTotalHoldDuration;

    private int mNumberOfPipesLight;
    private float mHoldTime;

    private void Update()
    {
        // assume mouse is down.

        mHoldTime += Time.deltaTime;
    }
}
