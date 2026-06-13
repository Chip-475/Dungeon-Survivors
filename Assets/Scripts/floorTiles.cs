using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class floorTiles : MonoBehaviour
{
    public static floorTiles instance;
    public Tilemap tilemap;
    public List<TileBase> tiles = new();
    public List<TileBase> roadTiles = new();
    [ContextMenu("Randomize Floor")]
    void randomizeFloor()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            int index = Random.Range(0, tiles.Count);
            if(tiles.Contains(tilemap.GetTile(pos)))
            tilemap.SetTile(pos, tiles[index]);
        }
    }

    [ContextMenu("Randomize Roads")]
    void randomizeRoads()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            int index = Random.Range(0, roadTiles.Count);
            if (roadTiles.Contains(tilemap.GetTile(pos)))
                tilemap.SetTile(pos, roadTiles[index]);
        }
    }
}
