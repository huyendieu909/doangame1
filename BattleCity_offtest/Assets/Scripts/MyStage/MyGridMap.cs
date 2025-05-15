using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyGridMap : MonoBehaviour
{
    [SerializeField]
    Tilemap brickTileMap, steelTileMap, tilePaletteTileMap;

    /*
        Đây là hai hàm cơ sở được dùng để đổi tile, các hàm chuyển tile sẽ sử 
        dụng các hàm này.
    */

    //Hàm thực hiện đổi 1 tile
    void UpdateAndRemoveTile(Vector3 position, TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        if (!(tileMapToRemoveFrom == steelTileMap && tileMapToRemoveFrom.GetTile(tileMapToRemoveFrom.WorldToCell(position)) == null))
        {
            tileMapToRemoveFrom.SetTile(tileMapToRemoveFrom.WorldToCell(position), null);
            tileMapToUpdate.SetTile(tileMapToUpdate.WorldToCell(position), tile);
        }
    }
    //Hàm thực hiện đổi tile cho cả căn cứ của P1
    void UpdateTileP1(TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
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
    //Hàm thực hiện đổi tile cho cả căn cứ của P2
    void UpdateTileP2(TileBase tile, Tilemap tileMapToRemoveFrom, Tilemap tileMapToUpdate)
    {
        UpdateAndRemoveTile(new Vector3(-2f, 12f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-2f, 11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-2f, 10f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(-1f, 10f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(0f, 10f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, 10f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, 11f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        UpdateAndRemoveTile(new Vector3(1f, 12f, 0), tile, tileMapToRemoveFrom, tileMapToUpdate);
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
        tileMapToUpdate.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
    }

    /*
        Các hàm thực hiện chuyển đổi giữa các loại brick và steel
    */

    //Hàm chuyển căn cứ sang Steel P1
    public void ChangeEagleWallToSteelP1()
    {
        Vector3 steelTilePosition = new Vector3(1f, 1f, 0);
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase steelTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(steelTilePosition));
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTileP1(steelTile, brickTileMap, steelTileMap);
    }
    //Hàm chuyển căn cứ sang Brick P1
    public void ChangeEagleWallToBrickP1()
    {
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTileP1(brickTile, steelTileMap, brickTileMap);
    }
    //Hàm chuyển căn cứ sang Steel P2
    public void ChangeEagleWallToSteelP2()
    {
        Vector3 steelTilePosition = new Vector3(1f, 1f, 0);
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase steelTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(steelTilePosition));
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTileP2(steelTile, brickTileMap, steelTileMap);
    }
    //Hàm chuyển căn cứ sang Brick P2
    public void ChangeEagleWallToBrickP2()
    {
        Vector3 brickTilePosition = new Vector3(0f, 0f, 0);
        TileBase brickTile = tilePaletteTileMap.GetTile(tilePaletteTileMap.WorldToCell(brickTilePosition));
        UpdateTileP2(brickTile, steelTileMap, brickTileMap);
    }

    // public void ActivateSpadePower()
    // {

    //     StartCoroutine(SpadePowerUpActivated());
    // }
    // IEnumerator SpadePowerUpActivated()
    // {
    //     StartCoroutine(ChangeEagleWallToSteelP1());
    //     yield return new WaitForSeconds(20f);
    //     ChangeEagleWallToBrickP1();
    // }
}
