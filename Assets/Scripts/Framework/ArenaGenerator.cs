using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ArenaDimensions
    {
        public int width;
        public int height;
    }

    [SerializeField]
    private ArenaDimensions dimensions;
    [SerializeField]
    private GameObject floorPrefab;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject spawnPrefab;

    private static ArenaGrid arenaGrid;
    public static ArenaGrid GetGridInstance()
    {
        return arenaGrid;
    }

    // Use this for initialization
    void Awake()
    {
        Vector2 wallBottomLeftCorner = (Vector2)Camera.main.ScreenToWorldPoint(Vector2.zero) + new Vector2(.5f, .5f);
        GenerateOuterWall(wallBottomLeftCorner, 0, dimensions.width - 1, 0, dimensions.height - 1);

        Vector2 arenaBottomLeftCorner = wallBottomLeftCorner + new Vector2(1, 1);
        arenaGrid = GenerateInnerArena(arenaBottomLeftCorner, 0, dimensions.width - 2, 0, dimensions.height - 2);
    }

    private void Start()
    {
        Events.OnEndAct += OnEndAct;
        Events.OnWaveStart += OnWaveStart;
    }

    private void OnDestroy()
    {
        Events.OnEndAct -= OnEndAct;
        Events.OnWaveStart -= OnWaveStart;
    }

    private void OnEndAct()
    {
        arenaGrid.ClearSpawnPoints();
    }

    private void OnWaveStart()
    {
        arenaGrid.ClearMutations();
        MutateMap(arenaGrid);
    }

    /*
     * Build the outer wall of the arena
     *
     * These pieces will not be monkeyed around with so we won't need a
     * reference to them after this returns
     */
    private void GenerateOuterWall(Vector2 bottomLeftCorner, int arenaLeftWall, int arenaRightWall, int arenaBottomWall, int arenaTopWall)
    {
        var offsetVector = new Vector2();
        for (int y = arenaBottomWall; y <= arenaTopWall; ++y)
        {
            offsetVector.x = arenaLeftWall;
            offsetVector.y = y;
            Instantiate(wallPrefab, bottomLeftCorner + offsetVector, Quaternion.identity, transform);
            offsetVector.x = arenaRightWall;
            Instantiate(wallPrefab, bottomLeftCorner + offsetVector, Quaternion.identity, transform);
        }

        for (int x = arenaLeftWall + 1; x <= arenaRightWall - 1; ++x)
        {
            offsetVector.y = arenaBottomWall;
            offsetVector.x = x;
            Instantiate(wallPrefab, bottomLeftCorner + offsetVector, Quaternion.identity, transform);
            offsetVector.y = arenaTopWall;
            Instantiate(wallPrefab, bottomLeftCorner + offsetVector, Quaternion.identity, transform);
        }
    }

    private ArenaGrid GenerateInnerArena(Vector2 bottomLeftCorner, int arenaLeft, int arenaRight, int arenaBottom, int arenaTop)
    {
        ArenaGrid grid = new ArenaGrid(bottomLeftCorner, arenaRight - arenaLeft, arenaTop - arenaBottom);

        var nextPos = bottomLeftCorner;
        for (int y = 0; y < grid.height; ++y)
        {
            for (int x = 0; x < grid.width; ++x)
            {
                grid.cells[x, y].floorTile = Instantiate(floorPrefab, nextPos, Quaternion.identity, transform);
                grid.cells[x, y].floorTile.SetActive(false);
                grid.cells[x, y].wallTile = Instantiate(wallPrefab, nextPos, Quaternion.identity, transform);
                grid.cells[x, y].wallEnabled(false);
                grid.cells[x, y].spawnTile = Instantiate(spawnPrefab, nextPos, Quaternion.identity, transform);
                grid.cells[x, y].spawnTile.SetActive(false);
                grid.ActivateFloor(x, y);
                nextPos.x += 1;
            }
            nextPos.x = bottomLeftCorner.x;
            nextPos.y += 1;
        }
        return grid;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MutateMap(arenaGrid);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            arenaGrid.ClearMutations(); 
        }
    }

    enum Mutation
    {
        L_Piece = 0,
        Flat,
        Square,
        S_Piece,
        T_Piece,
        NumMutations
    }

    private static readonly int MutationCount = 10;
    private static readonly int MaxMutationAttempts = 100;
    private void MutateMap(ArenaGrid arena)
    {
        int mutations = 0;
        int mutationAttempts = 0;
        while (mutations < MutationCount && mutationAttempts++ < MaxMutationAttempts)
        {
            Mutation nextMutation = (Mutation)Random.Range(0, (int)Mutation.NumMutations);
            Vec2i[] offsets = GetMutationOffsets(nextMutation);
            Vec2i randomGridPosition = arena.RandomCell();
            if (arena.MutationIsValid(randomGridPosition, offsets))
            {
                if (arena.AttemptMutateRegion(randomGridPosition, offsets))
                {
                    mutations += 1;
                }
            }
        }

        Debug.Log("MutationAttempts = " + mutationAttempts);
    }

    private Vec2i[] GetMutationOffsets(Mutation mutation)
    {
        Vec2i[] offsets = new Vec2i[4];
        switch (mutation)
        {
            case Mutation.L_Piece:
                offsets[0] = Vec2i.Zero;
                offsets[2] = Vec2i.Down;
                offsets[3] = Vec2i.Down * 2;
                offsets[1] = Vec2i.Down * 2 + Vec2i.Right;
                break;
            case Mutation.Flat:
                offsets[0] = Vec2i.Zero;
                offsets[1] = Vec2i.Right;
                offsets[2] = Vec2i.Right * 2;
                offsets[3] = Vec2i.Right * 3;
                break;
            case Mutation.Square:
                offsets[0] = Vec2i.Zero;
                offsets[1] = Vec2i.Right;
                offsets[2] = Vec2i.Down;
                offsets[3] = Vec2i.Down + Vec2i.Right;
                break;
            case Mutation.S_Piece:
                offsets[0] = Vec2i.Zero;
                offsets[1] = Vec2i.Down;
                offsets[2] = Vec2i.Down + Vec2i.Right;
                offsets[3] = Vec2i.Down * 2 + Vec2i.Right;
                break;
            case Mutation.T_Piece:
                offsets[0] = Vec2i.Zero;
                offsets[1] = Vec2i.Right;
                offsets[2] = Vec2i.Right * 2;
                offsets[3] = Vec2i.Right + Vec2i.Down;
                break;
            default:
                break;
        }
        return offsets;
    }

}

public struct ArenaCell
{
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject spawnTile;
    private bool _wallEnabled;

    public void wallEnabled(bool enabled)
    {
        wallTile.GetComponent<SpriteRenderer>().enabled = enabled;
        wallTile.GetComponent<BoxCollider2D>().enabled = enabled;
        _wallEnabled = enabled;
    }

    public bool IsWallEnabled()
    {
        return _wallEnabled;
    }
}

public struct ArenaRegion
{
    public ArenaRegion(Vec2i origin, Vec2i[] pieces)
    {
        this.origin = origin;
        this.pieces = pieces;
    }

    public Vec2i[] pieces;
    public Vec2i origin;
}

public class ArenaGrid
{
    public int width;
    public int height;
    public ArenaCell[,] cells;
    public Vector2 origin;
    private HashSet<ArenaRegion> mutations;
    private int floorCellCount = 0;
    private bool[,] visitedNodes;
    private List<Vec2i> spawnPoints;

    public ArenaGrid(Vector2 origin, int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new ArenaCell[width, height];
        this.origin = origin;
        mutations = new HashSet<ArenaRegion>();
        visitedNodes = new bool[width, height];
        spawnPoints = new List<Vec2i>(10);
    }

    public void ActivateFloor(int x, int y)
    {
        if (! cells[x,y].floorTile.activeSelf)
        {
            floorCellCount += 1;
        }
        cells[x,y].floorTile.SetActive(true);
        cells[x,y].wallEnabled(false);
    }

    public void ActivateWall(int x, int y)
    {
        if (cells[x,y].floorTile.activeSelf)
        {
            floorCellCount -= 1;
        }
        cells[x,y].floorTile.SetActive(false);
        cells[x,y].wallEnabled(true);
    }

    public void AddSpawnPoints(int numPoints)
    {
        int numAttempts = 100;
        while(numPoints > 0 && --numAttempts > 0)
        {
            Vec2i randomPos = RandomCell();
            ArenaCell randomCell = cells[randomPos.x, randomPos.y];
            if (! randomCell.IsWallEnabled() &&
                ! randomCell.spawnTile.activeSelf)
            {
                randomCell.spawnTile.SetActive(true);
                spawnPoints.Add(randomPos);
                --numPoints;
            }
        }

        if (numAttempts == 0)
        {
            Debug.LogError("ArenaGrid::AddSpawnPoints - Failed to add <" + numPoints + "> spawn points");
        }
    }

    public void ClearSpawnPoints()
    {
        foreach (Vec2i spawnPointPos in spawnPoints)
        {
            cells[spawnPointPos.x, spawnPointPos.y].spawnTile.SetActive(false);
        }
        spawnPoints.Clear();
        UnityEngine.Assertions.Assert.IsTrue(spawnPoints.Count == 0);
    }

    public Vector2 GetRandomSpawn()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        Vec2i spawnCellPos = spawnPoints[spawnIndex];
        return cells[spawnCellPos.x, spawnCellPos.y].spawnTile.transform.position;
    }

    public bool IsSpawnActive(int x, int y)
    {
        return cells[x, y].spawnTile.activeSelf;
    }

    public Vec2i RandomCell()
    {
        return new Vec2i(Random.Range(0, width), Random.Range(0, height));
    }

    public bool AttemptMutateRegion(Vec2i regionOrigin, Vec2i[] regionPieces)
    {
        ArenaRegion region = new ArenaRegion(regionOrigin, regionPieces);
        foreach (Vec2i regionPiece in regionPieces)
        {
            ActivateWall(regionOrigin.x + regionPiece.x, regionOrigin.y + regionPiece.y);
        }
        mutations.Add(region);
        if (!IsArenaStillConnected())
        {
            mutations.Remove(region);
            foreach (Vec2i regionPiece in regionPieces)
            {
                ActivateFloor(regionOrigin.x + regionPiece.x, regionOrigin.y + regionPiece.y);
            }
            return false;
        }
        return true;
    }


    public void ClearMutations()
    {
        foreach (ArenaRegion region in mutations)
        {
            foreach (Vec2i piece in region.pieces)
            {
                ActivateFloor(region.origin.x + piece.x, region.origin.y + piece.y);
            }
        }
        mutations.Clear();
    }

    public bool MutationIsValid(Vec2i mutationOrigin, Vec2i[] mutationRegion)
    {
        LayerMask playerAndEnemyMask = LayerMask.GetMask("Player", "Enemy");
        foreach (var mutationPiecePos in mutationRegion)
        {
            var mutationPosition = mutationPiecePos + mutationOrigin;
            if (IsOutOfBounds(mutationPosition.x, mutationPosition.y) || IsSpawnActive(mutationPosition.x, mutationPosition.y))
            {
                return false;
            }

            var potentialWallPos = mutationPosition + origin; 
            if (Physics2D.BoxCast(potentialWallPos, new Vector2(1, 1), 0, Vector2.zero, 0, playerAndEnemyMask))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsOutOfBounds(int x, int y)
    {
        return (x >= width || x < 0) || (y >= height || y < 0);
    }

    public bool IsArenaStillConnected()
    {
        /** find a floor tile to start at **/
        int startX = -1;
        int startY = -1;
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (cells[x, y].floorTile.activeSelf)
                {
                    startX = x;
                    startY = y;
                    // exit early
                    x = width;
                    y = height;
                }
            }
        }
        UnityEngine.Assertions.Assert.IsTrue(startX != -1 && startY != -1);
        System.Array.Clear(visitedNodes, 0, visitedNodes.Length);
        var count = GetConnectedCountRecursive(startX, startY);
        return floorCellCount == count;
    }

    private int GetConnectedCountRecursive(int x, int y)
    {
        if (IsOutOfBounds(x, y) || cells[x,y].IsWallEnabled() || visitedNodes[x, y])
        {
            return 0;
        }

        visitedNodes[x, y] = true;

        return 1 +
               GetConnectedCountRecursive(x + 1, y) +
               GetConnectedCountRecursive(x, y + 1) +
               GetConnectedCountRecursive(x - 1, y) +
               GetConnectedCountRecursive(x, y - 1);
    }
}
