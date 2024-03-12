using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    [SerializeField] private PatternData[] rowsMap;
    private int rows;
    [SerializeField] private int cols;
    private int[,] tilesMap;
    private float tileWidth;
    private float tileHeight;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;
    [SerializeField] private Transform bottomRight;
    [SerializeField] private Transform topLeft;

    void Awake()
    {
        rows = rowsMap.Length;
        // set transforms to camera min and max
        topRight.position = new Vector3(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        bottomLeft.position = topRight.position * -1;

        topLeft.position = Vector3.Scale(topRight.position, new Vector3(-1, 1, 0));
        bottomRight.position = topRight.position * -1;

        //calculate tile width and height
        tileHeight = (topRight.position.y - bottomLeft.position.y)/rows;
        tileWidth = (topRight.position.x - bottomLeft.position.x)/cols;

        // init tile map size; tile map will keep track of all tiles on screen
        // to see which tiles can be walked on and which are trees that cant be moved to
        tilesMap = new int[rows,cols];

        // spawn tiles on map
        for (int i = 0; i < rows; i++)
        {
            PatternData currRow = rowsMap[i];
            int[] tilesInRow = currRow.tiles; // holds ints representing tile from its tileOrder array
            GameObject[] tiles = currRow.tileOrder;
            for (int j = 0; j < cols; j++)
            {
                int currTileIndex = tilesInRow[j % tilesInRow.Length];
                float tileX = tileWidth*j + bottomLeft.position.x + (tileWidth / 2);
                float tileY = tileHeight*i + bottomLeft.position.y + (tileHeight / 2);
                GameObject tileObj = Instantiate(tiles[currTileIndex], new Vector3(tileX,tileY,0), Quaternion.identity);
                Vector3 spriteSize = tileObj.GetComponent<SpriteRenderer>().bounds.size;
                tileObj.transform.localScale = new Vector3(tileWidth/spriteSize.x, tileHeight/spriteSize.y);
                int tileCategory;
                if (tileObj.CompareTag("Not Walkable"))
                {
                    tileCategory = 1;
                } else if (tileObj.CompareTag("Slippery"))
                {
                    tileCategory = 2;
                } else {
                    tileCategory = 0;
                }
                tilesMap[i,j] = tileCategory;
            }
        }
    }


    public bool canMoveToTile(Vector3 nextPlayerCenter)
    {
        Vector2Int nextPlayerTilePos = getTileCurrent(nextPlayerCenter);
        // Debug.Log(nextPlayerTilePos);
        // this code is meant to fix an error that happened when the player
        // tried to go out of bounds because those values were out of range
        // of tilesMap; exit early instead
        if (nextPlayerTilePos.x < 0 || nextPlayerTilePos.x > rows - 1 ||
            nextPlayerTilePos.y < 0 || nextPlayerTilePos.y > cols - 1)
        {
            return false;
        }
        // a tile marked as 1 is not walkable
        return tilesMap[nextPlayerTilePos.x, nextPlayerTilePos.y] != 1;
    }

    private bool isOnWinTile(Vector2Int playerTilePos)
    {
        // Vector2Int playerTilePos = getTileCurrent(playerCenter);
        // return (playerTilePos.x == rows - 1) && (playerTilePos.y == cols - 1);
        // temporarily changing it to be anywhere on the last row
        // for it to be easier to understand
        return (playerTilePos.x == rows - 1);
    }

    // checks current tile for special effects such as win tile or ice tile
    public void checkTileEffects(Vector3 playerCenter, PlayerMovement playerScript) {
        Vector2Int playerTilePos = getTileCurrent(playerCenter);
        if (isOnWinTile(playerTilePos)) {
            playerScript.Win();
        } else if (tilesMap[playerTilePos.x, playerTilePos.y] == 2) {
            playerScript.IceSlide();
        }
    }

    public Vector3 getTileSize()
    {
        return new Vector3(tileWidth, tileHeight);
    }
    public Vector3 getTileCount()
    {
        return new Vector3(cols, rows);
    }
    // returns the tile the given position is located on
    // in row,col form
    public Vector2Int getTileCurrent(Vector3 playerCenter)
    {
        // this code is meant to fix an error that made the result
        // one tile too high if it was negative
        // int isNegativeX = (int)((int)playerCenter.x / (int)Mathf.Abs(playerCenter.x));
        // float accuracyOffsetX = 0.00001f * isNegativeX;
        // int isNegativeY = (int)((int)playerCenter.y / (int)Mathf.Abs(playerCenter.y));
        // float accuracyOffsetY = 0.00001f * isNegativeY;

        // calculate col and row of tile that given position is located on
        int playerCol = Mathf.FloorToInt((playerCenter.x - bottomLeft.position.x - (tileWidth / 2)) / tileWidth);
        int playerRow = Mathf.FloorToInt((playerCenter.y - bottomLeft.position.y - (tileHeight / 2)) / tileHeight);
        return new Vector2Int(playerRow, playerCol);
    }
    public Vector3 getBottomLeft()
    {
        return bottomLeft.position;
    }
    public Vector3 getTopRight()
    {
        return topRight.position;
    }
    public Vector3 getBottomRight()
    {
        return bottomRight.position;
    }
    public Vector3 getTopLeft()
    {
        return topLeft.position;
    }
}
