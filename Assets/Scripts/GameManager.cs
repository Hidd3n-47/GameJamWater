using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
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
}
