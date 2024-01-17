using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpForce = 1000;

    [Networked] private NetworkButtons buttonsPrevs { get; set; }
    private float horizontal;
    private Rigidbody2D playerRigid;

    private enum PlayerInputButtons
    {
        None,
        Jump
    }

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
            CheckJumpInput(input);
        }
    }   

    private void CheckJumpInput(PlayerData input)
    {
        var pressed = input.networkButtons.GetPressed(buttonsPrevs);

        if(pressed.WasPressed(buttonsPrevs, PlayerInputButtons.Jump))
        {
            playerRigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        buttonsPrevs = input.networkButtons;
    }

    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data = new PlayerData();
        data.horizontalInput = horizontal;
        data.networkButtons.Set(PlayerInputButtons.Jump, Input.GetKey(KeyCode.Space));
        return data;
    }
}