using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public String NextScene = "Day";

    public UnityEvent OnFailedGame;

    [SerializeField] private Transform t;

    [SerializeField]
    private AudioSource mEndAudio;

    [SerializeField] 
    public float mTotalTimeForDay = 60.0f;

    public float mDayTimer;

    [SerializeField]
    public float mStartShiftTime = 9.0f;
    [SerializeField]
    public float mEndShiftTime = 17.0f;
    [SerializeField]
    private TextMeshProUGUI mClockTimer;

    [Header("Values to tweak")]
    [SerializeField]
    private float mPercentageForPassing = 100.0f;
    [SerializeField]
    private float mPercentageLostForFailing = 20.0f;
    [SerializeField]
    private float mPercentageLostForDoingModuleTooSoon = 20.0f;

    [Header("Info for puzzles (Order Matters!)")]
    [SerializeField]
    List<Material> mColors;

    [SerializeField]
    List<Town> mTowns;

    [SerializeField]
    List<MeshRenderer> mTownBackPlanes;

    [SerializeField]
    List<IModule> mModules;

    public float GameTime => mDayTimer;

    private readonly Dictionary<int, Town> mIdToTown = new();

    private readonly List<int> mColorIdOfModules = new();

    public void OnModuleCompleted(IModule module, int moduleId, bool passed)
    {
        Town town = mIdToTown[moduleId];
        if (town.CanFix && passed)
        {
            Debug.Log("Well done you fixed it.");
            module.Passed();
            town.IncreasePercentage(mPercentageForPassing);
        }
        else if(passed)
        {
            Debug.Log("Town not ready to fix, face the consequences.");
            module.Failed();
            town.DecreasePercentage(mPercentageLostForDoingModuleTooSoon);
        }
        else
        {
            Debug.Log("You just failed, skill issue.");
            module.Failed();
            town.DecreasePercentage(mPercentageLostForFailing);
        }
    }

    private void Start()
    {
        GenerateRandomColorForTowns();

        GenerateRandomColorForPuzzle();

        for (int i = 0; i < mModules.Count; ++i)
        {
            if(!mModules[i]) continue;

            mModules[i].ModuleId = mColorIdOfModules[i];
            mModules[i].Register(this, mColorIdOfModules[i]);
            Debug.Log(i + " - " + mColorIdOfModules[i]);
        }

        (mModules[0] as PasswordModule)?.SetPassword(mIdToTown[mColorIdOfModules[0]].Password);
    }

    private void GenerateRandomColorForTowns()
    {
        Random rnd = new Random();

        var randomUniqueList = Enumerable.Range(0, mTowns.Count)
            .OrderBy(_ => rnd.Next())
            .ToList();

        foreach (Town t in mTowns)
        {
            t.GetComponentInChildren<MeshRenderer>().material = mColors[randomUniqueList[0]];

            mIdToTown[randomUniqueList[0]] = t;

            Debug.Log(randomUniqueList[0] + " - " + t.transform.parent.name);

            randomUniqueList.Remove(randomUniqueList[0]);
        }
    }

    private void GenerateRandomColorForPuzzle()
    {
        Random rnd = new Random();

        var randomUniqueList = Enumerable.Range(0, mTowns.Count)
            .OrderBy(_ => rnd.Next())
            .ToList();

        foreach (MeshRenderer mesh in mTownBackPlanes)
        {
            mesh.material = mColors[randomUniqueList[0]];

            mColorIdOfModules.Add(randomUniqueList[0]);

            randomUniqueList.Remove(randomUniqueList[0]);
        }
    }

    private void Update()
    {
        mDayTimer += Time.deltaTime;

        float time = math.lerp(mStartShiftTime, mEndShiftTime, mDayTimer / mTotalTimeForDay);

        int minutes = (int)((time - (int)time) * 60.0f);

        mClockTimer.text = ((int)time) + ":" + (minutes < 10 ? "0" + minutes : minutes);

        if (time > mEndShiftTime)
        {
            Debug.Log("get out of here time.");
        }
    }

    private bool failing = false;

    public void OnGameFailed()
    {
        if (failing) return;

        failing = true;
        StartCoroutine(GameLost());
    }

    IEnumerator GameLost()
    {
        Time.timeScale = 0.0f;

        float timer = 0.0f;

        mEndAudio.Play();

        while (timer < mEndAudio.clip.length)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadSceneAsync("Day1");
    }
}
