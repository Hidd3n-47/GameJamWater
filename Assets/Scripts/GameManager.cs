using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IModule[] mModules = { null, null, null, null, null, null };

    [SerializeField]
    private List<Transform> mModulesList;

    private void Start()
    {
        GenerateTable();
    }

    private void GenerateTable()
    {
        // For now, just generate a password module in position 4 (2nd row, 2nd column).
        Transform moduleGameObject = Instantiate(mModulesList[0]);
        mModules[4] = moduleGameObject.GetComponent<IModule>();
    }
}
