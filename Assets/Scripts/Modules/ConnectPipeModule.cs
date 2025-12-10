using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConnectPipeModule : IModule
{
    [SerializeField]
    int mRows = 3;

    [SerializeField]
    int mColumns = 3;

    [SerializeField]
    Transform mParentOfSpawnPoints;
    Transform[] mSpawnPoints;

    private enum CardinalDirection
    {
        NONE,
        NORTH =  1,
        EAST  =  2,
        SOUTH = -1,
        WEST  = -2
    }

    // Order TopLeft, Horizontal, TopRight, BottomLeft, BottomRight
    [SerializeField] 
    private Transform[] mConnections = { null, null };

    private struct SolutionNode
    {
        public Transform transform;
        public float rotation;
        public CardinalDirection direction;
        public bool partOfSolution;
    }

    private SolutionNode[,] mSolution = { };

    private void Start()
    {
        mSpawnPoints = mParentOfSpawnPoints.GetComponentsInChildren<Transform>().Where(x => x != mParentOfSpawnPoints).ToArray();

        GenerateSolution();
    }

    private void GenerateSolution()
    {
        Vector2Int startingPosition = new Vector2Int(0, 0);
        Vector2Int currentNodePosition = startingPosition;
        Vector2Int finalPosition = new Vector2Int(mRows - 1, mColumns - 1);

        mSolution = new SolutionNode[mRows, mColumns];

        CardinalDirection previousDirection = CardinalDirection.SOUTH;
        while (currentNodePosition != finalPosition)
        {
            List<CardinalDirection> possibleDirections = GetCardinalDirections(currentNodePosition, startingPosition, previousDirection);

            CardinalDirection direction = possibleDirections[Random.Range(0, possibleDirections.Count)];
            mSolution[currentNodePosition.x, currentNodePosition.y] = new SolutionNode() { direction = direction, partOfSolution = true};

            currentNodePosition += GetPositionFromCardinalDirection(direction);
            previousDirection = direction;
        }

        mSolution[finalPosition.x, finalPosition.y] = new SolutionNode() { direction = CardinalDirection.SOUTH, partOfSolution = true };

        previousDirection = CardinalDirection.SOUTH;
        for (int y = 0; y < mColumns; ++y)
        {
            for (int x = 0; x < mRows; ++x)
            {
                if (!mSolution[x, y].partOfSolution)
                {
                    continue;
                }

                bool sameDirection = mSolution[x, y].direction == previousDirection;

                mSolution[x, y].transform = sameDirection ? mConnections[0] : mConnections[1];
                previousDirection = mSolution[x, y].direction;
            }
        }

        SpawnPipes();
    }
    private static bool AdjacentToEndPoint(Vector2Int point, Vector2Int end)
    {
        Vector2Int delta = end - point;
        return Math.Abs(delta.x) + Math.Abs(delta.y) == 1;
    }

    private List<CardinalDirection> GetCardinalDirections(Vector2Int position, Vector2Int startingPosition, CardinalDirection previousDirection)
    {
        if (position == startingPosition)
        {
            return new List<CardinalDirection>() { CardinalDirection.EAST, CardinalDirection.SOUTH };
        }

        var cardinalDirections = new List<CardinalDirection>() { CardinalDirection.NORTH, CardinalDirection.EAST, CardinalDirection.SOUTH, CardinalDirection.WEST };

        if (position.x == 0)
        {
            cardinalDirections.Remove(CardinalDirection.WEST);

            // Remove North to ensure there will always be a solution
            cardinalDirections.Remove(CardinalDirection.NORTH);
        }
        else if (position.x == mRows - 1)
        {
            cardinalDirections.Remove(CardinalDirection.EAST);

            // Remove North to ensure there will always be a solution
            cardinalDirections.Remove(CardinalDirection.NORTH);
        }

        if (position.y == 0)
        {
            cardinalDirections.Remove(CardinalDirection.NORTH);

            // Remove West to ensure there will always be a solution
            cardinalDirections.Remove(CardinalDirection.WEST);
        }
        else if (position.y == mColumns - 1)
        {
            cardinalDirections.Remove(CardinalDirection.SOUTH);

            // Remove West to ensure there will always be a solution
            cardinalDirections.Remove(CardinalDirection.WEST);
        }

        CardinalDirection oppositeDirection = GetOppositeDirection(previousDirection);

        var possibilities = new List<CardinalDirection>();
        foreach (CardinalDirection dir in cardinalDirections)
        {
            Vector2Int updatedPosition = position + GetPositionFromCardinalDirection(dir);

            if (!mSolution[updatedPosition.x, updatedPosition.y].partOfSolution && dir != oppositeDirection)
            {
                possibilities.Add(dir);
            }
        }

        return possibilities;
    }

    private static Vector2Int GetPositionFromCardinalDirection(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.NORTH: return new Vector2Int( 0, -1);
            case CardinalDirection.EAST : return new Vector2Int( 1,  0);
            case CardinalDirection.SOUTH: return new Vector2Int( 0,  1);
            case CardinalDirection.WEST : return new Vector2Int(-1,  0);
        }

        return new Vector2Int(0, 0);
    }

    private static CardinalDirection GetOppositeDirection(CardinalDirection direction)
    {
        return (CardinalDirection)((int)direction * -1);
    }

    private void SpawnPipes()
    {
        CardinalDirection previousDirection = CardinalDirection.SOUTH;
        for (int y = 0; y < mColumns; ++y)
        {
            for (int x = 0; x < mRows; ++x)
            {
                Transform instance;
                if (mSolution[x, y].partOfSolution)
                {
                    int vertical = CardinalDirectionParallel(mSolution[x, y].direction, previousDirection) ? 0 : 1;
                    Instantiate(mConnections[vertical], mSpawnPoints[y * mColumns + x]);
                    previousDirection = mSolution[x, y].direction;
                }
                else
                {
                    //instance = mConnections[Random.Range(0, mConnections.Length - 1)];
                }

                //Instantiate(instance, mSpawnPoints[y * 7 + x]);
            }
        }
    }

    static bool CardinalDirectionParallel(CardinalDirection dir1, CardinalDirection dir2)
    {
        return Math.Abs((int)dir1) == Math.Abs((int)dir2);
    }
}
