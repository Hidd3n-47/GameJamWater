using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConnectPipeModule : IModule
{
    [SerializeField] private int mRows = 3;
    [SerializeField] private int mColumns = 3;

    [SerializeField] private Transform mParentOfSpawnPoints;
    private Transform[] mSpawnPoints;

    private Transform[,] mInstances;

    [SerializeField]
    private Transform[] mConnections = { null, null };

    private int mNumberOfPipes = -1;
    private int mNumberCorrect = 0;

    private struct SolutionNode
    {
        public Transform prefab;
        public float rotation;
        public bool partOfSolution;
    }

    private SolutionNode[,] mSolution;


    private enum CardinalDirection
    {
        NONE = 0,
        NORTH = 1,
        EAST = 2,
        SOUTH = -1,
        WEST = -2
    }

    private void Start()
    {
        mSpawnPoints = mParentOfSpawnPoints.GetComponentsInChildren<Transform>().Where(t => t != mParentOfSpawnPoints).ToArray();

        InitPuzzle();;
    }

    protected override void InitPuzzle()
    {
        mInstances = new Transform[mRows, mColumns];
        GenerateSolution();
    }

    protected override void DestroyPuzzle()
    {
        for (int y = 0; y < mColumns; y++)
        {
            for (int x = 0; x < mRows; x++)
            {
                SolutionNode node = mSolution[x, y];
                if (!node.partOfSolution)
                {
                    continue;
                }

                Destroy(mInstances[x,y].gameObject);
            }
        }
    }

    public void PipeInCorrectPosition()
    {
        ++mNumberCorrect;

        if (mNumberCorrect >= mNumberOfPipes)
        {
            OnPassedEventHandler?.Invoke();
        }
    }

    public void PipeRemovedFromCorrectPosition()
    {
        --mNumberCorrect;
    }

    protected override void OnPassed()
    {
        Debug.Log("Completed module");
    }

    private void GenerateSolution()
    {
        mSolution = new SolutionNode[mRows, mColumns];
        List<Vector2Int> path = GenerateRandomPath();

        mNumberOfPipes = path.Count;
        mNumberCorrect = 0;

        AssignPipeShapes(path);
        SpawnPipes();
    }

    private List<Vector2Int> GenerateRandomPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int current = new Vector2Int(0, 0);
        Vector2Int goal = new Vector2Int(mRows - 1, mColumns - 1);

        path.Add(current);

        while (current != goal)
        {
            List<Vector2Int> options = new List<Vector2Int>();

            if (current.x + 1 < mRows)
            {
                options.Add(new Vector2Int(current.x + 1, current.y));
            }

            if (current.y + 1 < mColumns)
            {
                options.Add(new Vector2Int(current.x, current.y + 1));
            }

            current = options[Random.Range(0, options.Count)];
            path.Add(current);
        }

        return path;
    }

    private void AssignPipeShapes(List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int pos = path[i];

            CardinalDirection inDir = CardinalDirection.SOUTH;
            if (i > 0)
            {
                inDir = GetDirection(path[i - 1], pos);
            }

            CardinalDirection outDir = CardinalDirection.SOUTH;
            if (i < path.Count - 1)
            {
                outDir = GetDirection(pos, path[i + 1]);
            }

            bool isStraight = Mathf.Abs((int)inDir) == Mathf.Abs((int)outDir);

            Transform prefab;
            float rotation = 0f;

            if (isStraight)
            {
                prefab = mConnections[0];

                if (inDir == CardinalDirection.NORTH || inDir == CardinalDirection.SOUTH)
                {
                    rotation = 0f;
                }
                else
                {
                    rotation = 90f;
                }
            }
            else
            {
                prefab = mConnections[1];
                rotation = DetermineCornerRotation(inDir, outDir);
            }

            mSolution[pos.x, pos.y] = new SolutionNode
            {
                prefab = prefab,
                rotation = rotation,
                partOfSolution = true
            };
        }
    }

    private void SpawnPipes()
    {
        for (int y = 0; y < mColumns; y++)
        {
            for (int x = 0; x < mRows; x++)
            {
                SolutionNode node = mSolution[x, y];
                if (!node.partOfSolution)
                {
                    continue;
                }

                Transform spawnPoint = mSpawnPoints[y * mRows + x];

                Transform instance = Instantiate(node.prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
                instance.GetComponent<Pipe>().Init(this, node.rotation, node.prefab == mConnections[1]);
                mInstances[x, y] = instance;
            }
        }
    }

    private static CardinalDirection GetDirection(Vector2Int from, Vector2Int to)
    {
        Vector2Int diff = to - from;

        if (diff.x == 1)  return CardinalDirection.EAST;
        if (diff.x == -1) return CardinalDirection.WEST;
        if (diff.y == 1)  return CardinalDirection.SOUTH;
        if (diff.y == -1) return CardinalDirection.NORTH;

        return CardinalDirection.NONE;
    }

    private float DetermineCornerRotation(CardinalDirection inDir, CardinalDirection outDir)
    {
        if (inDir == CardinalDirection.EAST && outDir == CardinalDirection.SOUTH)
        {
            return 0.0f;
        }

        if (inDir == CardinalDirection.EAST && outDir == CardinalDirection.NORTH)
        {
            return 90.0f;
        }

        if (inDir == CardinalDirection.SOUTH && outDir == CardinalDirection.EAST)
        {
            return 180.0f;
        }

        if (inDir == CardinalDirection.NORTH && outDir == CardinalDirection.EAST)
        {
            return 270.0f;
        }

        return 0f;
    }
}
