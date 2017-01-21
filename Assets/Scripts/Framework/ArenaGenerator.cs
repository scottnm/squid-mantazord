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

    private ArenaGrid arenaGrid;

    // Use this for initialization
    void Awake()
    {
        Vector2 wallBottomLeftCorner = (Vector2)Camera.main.ScreenToWorldPoint(Vector2.zero) + new Vector2(.5f, .5f);
        GenerateOuterWall(wallBottomLeftCorner, 0, dimensions.width - 1, 0, dimensions.height - 1);

        Vector2 arenaBottomLeftCorner = wallBottomLeftCorner + new Vector2(1, 1);
        arenaGrid = GenerateInnerArena(arenaBottomLeftCorner, 0, dimensions.width - 2, 0, dimensions.height - 2);
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
                grid.cells[x, y].wallTile = Instantiate(wallPrefab, nextPos, Quaternion.identity, transform);
                grid.cells[x, y].wallTile.SetActive(false);
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

    private static readonly int MutationCount = 1;
    private void MutateMap(ArenaGrid arena)
    {
        int mutations = 0;
        while (mutations < MutationCount)
        {
            Mutation nextMutation = (Mutation)Random.Range(0, (int)Mutation.NumMutations);
            Vec2i[] offsets = GetMutationOffsets(nextMutation);
            Vec2i randomGridPosition = arenaGrid.RandomCell();
            if (arena.MutationIsValid(randomGridPosition, offsets))
            {
                arena.MutateRegion(randomGridPosition, offsets);
                mutations += 1;
            }
        }
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

    public ArenaGrid(Vector2 origin, int width, int height)
    {
        this.width = width;
        this.height = height;
        cells = new ArenaCell[width, height];
        this.origin = origin;
        mutations = new HashSet<ArenaRegion>();
    }

    public void ActivateFloor(int x, int y)
    {
        cells[x, y].floorTile.SetActive(true);
        cells[x, y].wallTile.SetActive(false);
    }

    public void ActivateWall(int x, int y)
    {
        cells[x, y].floorTile.SetActive(false);
        cells[x, y].wallTile.SetActive(true);
    }

    public bool IsWallActive(int x, int y)
    {
        return cells[x, y].wallTile.activeSelf;
    }

    public Vec2i RandomCell()
    {
        return new Vec2i(Random.Range(0, width), Random.Range(0, height));
    }

    public void MutateRegion(Vec2i regionOrigin, Vec2i[] regionPieces)
    {
        ArenaRegion region = new ArenaRegion(regionOrigin, regionPieces);
        foreach (Vec2i regionPiece in regionPieces)
        {
            ActivateWall(regionOrigin.x + regionPiece.x, regionOrigin.y + regionPiece.y);
        }
        mutations.Add(region);
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
            if ((mutationPosition.x >= width || mutationPosition.x < 0) ||
                 (mutationPosition.y >= height || mutationPosition.y < 0))
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
}
