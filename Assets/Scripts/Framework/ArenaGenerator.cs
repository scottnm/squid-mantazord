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

    public class ArenaGrid
    {
        public ArenaCell[,] cells;
        public int width;
        public int height;
    }

    public struct ArenaCell
    {
        public GameObject floorTile;
        public GameObject wallTile;
    }

    [SerializeField]
    ArenaDimensions dimensions;
    [SerializeField]
    GameObject floorPrefab;
    [SerializeField]
    GameObject wallPrefab;
    
	// Use this for initialization
	void Start ()
    {
        Vector2 wallBottomLeftCorner = (Vector2) Camera.main.ScreenToWorldPoint(Vector2.zero) + new Vector2(.5f, .5f);
        GenerateOuterWall(wallBottomLeftCorner, 0, dimensions.width - 1, 0, dimensions.height - 1);

        Vector2 arenaBottomLeftCorner = wallBottomLeftCorner + new Vector2(1, 1);
        GenerateInnerArena(arenaBottomLeftCorner, 0, dimensions.width - 2, 0, dimensions.height - 2);
	}

    /*
     * Build the outer wall of the arena
     *
     * These pieces will not be monkeyed around with so we won't need a
     * reference to them after this returns
     */
    private void GenerateOuterWall(Vector2 bottomLeftCorner, int arenaLeftWall, int arenaRightWall, int arenaBottomWall, int arenaTopWall)
    {
        /**** OUTER WALL CREATION ****/
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
        ArenaGrid grid = new ArenaGrid();
        grid.cells = new ArenaCell[arenaRight - arenaLeft, arenaTop - arenaBottom];
        grid.width = arenaRight - arenaLeft;
        grid.height = arenaTop - arenaBottom;

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
	void Update ()
    {
		
	}
}
