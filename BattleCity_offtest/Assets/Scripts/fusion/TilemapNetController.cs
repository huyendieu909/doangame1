using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Fusion;

public class TilemapNetController : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    // RPC để xóa tile trên tất cả client
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DestroyTile(Vector3Int cellPosition)
    {
        if (tilemap.HasTile(cellPosition))
        {
            tilemap.SetTile(cellPosition, null);
            tilemap.RefreshTile(cellPosition);
        }
    }
}
