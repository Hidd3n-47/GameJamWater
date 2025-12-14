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
    public MeshRenderer A;
    public MeshRenderer B;
    public MeshRenderer C;
    public MeshRenderer D;
    public MeshRenderer AA;
    public MeshRenderer BB;
    public MeshRenderer CC;
    public MeshRenderer DD;
    public MeshRenderer[] lights;
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
        Destroy(ticking);
        Destroy(musicManager);

        float timer = 0.0f;

        var a = GetComponent<AudioSource>();
            a.Play();

            {

                var materials = A.materials;
                materials[1] = matWhite;
                A.materials = materials;

                A.gameObject.SetActive(false);
        }

        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        C.gameObject.SetActive(false);
        D.gameObject.SetActive(false);

        AA.gameObject.SetActive(true);
        BB.gameObject.SetActive(true);
        CC.gameObject.SetActive(true);
        DD.gameObject.SetActive(true);


        foreach (var f in lights)
        {
            var materials = f.materials;
            materials[1] = matWhite;
            f.materials = materials;
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
