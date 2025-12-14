using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public AudioSource ticking;
    public GameObject musicManager;

    public List<TextMeshProUGUI> mapText = new List<TextMeshProUGUI>();
    public List<Transform> lights = new List<Transform>();
    public Material matWhite;

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

        Destroy(ticking);
        Destroy(musicManager);

        float timer = 0.0f;

        var a = GetComponent<AudioSource>();
            a.Play();


        foreach (var f in lights)
        {
            var mesh = f.GetComponent<MeshRenderer>();
            var materials = mesh.materials;
            materials[1] = matWhite;
            mesh.materials = materials;
        }

        foreach (var q in mapText)
        {
            q.enabled = false;
        }

        while (timer < a.clip.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync(mGameManager.NextScene);
    }
}
