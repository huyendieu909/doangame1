using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local {get; set;}
    public override void Spawned()
    {
        // base.Spawned();
        if (Object.HasInputAuthority) {
            Local = this;
            Debug.Log("Spawn mine");
        } else Debug.Log("Spawn other ");
    }
    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority) {
            Runner.Despawn(Object);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


}
