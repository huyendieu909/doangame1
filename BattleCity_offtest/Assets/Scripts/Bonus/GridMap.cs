using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    [SerializeField]
    Tilemap brickTileMap, steelTileMap, tilePaletteTileMap;

    void UpdateAndRemoveTile(Vector3 position, TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        if (!(tileMapToRemoveFrom == steelTileMap && tileMapToRemoveFrom.GetTile(tileMapToRemoveFrom.WorldToCell(position)) == null))
        {
            tileMapToRemoveFrom.SetTile(tileMapToRemoveFrom.WorldToCell(position), null);
            tileMapToUpdate.SetTile(tileMapToUpdate.WorldToCell(position), tile);
        }
    }
    void UpdateTile(TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        UpdateAndRemoveTile(new Vector3(-2f, -13f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-2f, -12f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-2f, -11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-1f, -11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(0f, -11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, -11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, -12f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, -13f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
    }
    IEnumerator ChangeEagleWallToSteel()
    {
        Vector3 steelTilePosition = new Vector3(1f, 1f, 0);
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase steelTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(steelTilePosition));
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTile(steelTile, brickTileMap, steelTileMap);
        yield return new WaitForSeconds(15f);
        for (int i = 0; i < 5; i++){
            UpdateTile(brickTile, steelTileMap, steelTileMap);
            yield return new WaitForSeconds(0.5f);
            UpdateTile(steelTile, steelTileMap, steelTileMap);
            yield return new WaitForSeconds(0.5f);
        }
    }
    void ChangeEagleWallToBrick()
    {
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTile(brickTile, steelTileMap, brickTileMap);
    }
    public void ActivateSpadePower()
    {
        StartCoroutine(SpadePowerUpActivated());
    }
    IEnumerator SpadePowerUpActivated()
    {
        StartCoroutine(ChangeEagleWallToSteel());
        yield return new WaitForSeconds(20f);
        ChangeEagleWallToBrick();
    }
}
