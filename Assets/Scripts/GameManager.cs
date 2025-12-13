using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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

    private void Start()
    {
        Random rnd = new Random();
        var randomUniqueList = Enumerable.Range(0, mTowns.Count)
            .OrderBy(x => rnd.Next())
            .ToList();

        foreach (Town t in mTowns)
        {
            t.GetComponentInChildren<MeshRenderer>().material = mColors[randomUniqueList[0]];

            randomUniqueList.Remove(randomUniqueList[0]);
        }
    }
}
