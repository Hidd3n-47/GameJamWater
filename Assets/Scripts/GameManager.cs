using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        GenerateRandomColorForTowns();

        GenerateRandomColorForPuzzle();

        for (int i = 0; i < mModules.Count; ++i)
        {
            if(!mModules[i]) continue;

            mModules[i].ModuleId = mColorIdOfModules[i];
            Debug.Log(i + " - " + mColorIdOfModules[i]);
        }
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
