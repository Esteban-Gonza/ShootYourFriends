using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] private float moveSpeed = 6;

    private float horizontal;
    private Rigidbody2D playerRigid;

    public override void Spawned()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    public void BeforeUpdate()
    {
        //Ask if it is the local machine
        if(Runner.LocalPlayer == Object.HasInputAuthority)
        {
            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxisRaw(HORIZONTAL);
        }
    }

    //FUN Function
    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input))
        {
            playerRigid.velocity = new Vector2(input.horizontalInput * moveSpeed, playerRigid.velocity.y);
        }
    }   

    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data = new PlayerData();
        data.horizontalInput = horizontal;
        return data;
    }
}