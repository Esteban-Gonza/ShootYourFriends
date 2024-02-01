using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject camera;
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpForce = 1000;

    [Networked(OnChanged = nameof(OnNicknameChanged))] private NetworkString<_8> playerName { get; set; }
    [Networked] private NetworkButtons buttonsPrevs { get; set; }

    private float horizontal;
    private Rigidbody2D playerRigid;
    private PlayerWeaponController playerWeaponController;
    private PlayerVisualController playerVisualController;

    private enum PlayerInputButtons
    {
        None,
        Jump
    }

    public override void Spawned()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        playerVisualController = GetComponent<PlayerVisualController>();

        SetLocalObjects();
    }

    private void SetLocalObjects()
    {
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            camera.SetActive(true);

            var nickname = GlobalManagers.Instance.networkRunnerController.localPlayerNickname;
            RpcSetNickname(nickname);
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetNickname(NetworkString<_8> nickname)
    {
        playerName = nickname;
    }

    private static void OnNicknameChanged(Changed<PlayerController> changed)
    {
        changed.Behaviour.SetPlayerNickname(changed.Behaviour.playerName);
    }

    private void SetPlayerNickname(NetworkString<_8> nickname)
    {
        playerNameText.text = nickname + " " + Object.InputAuthority.PlayerId;
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

        playerVisualController.UpdateScaleTransforms(playerRigid.velocity);
    }

    public override void Render()
    {
        playerVisualController.RendererVisuals(playerRigid.velocity);
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
        data.gunPivotRotation = playerWeaponController.localQuaternionPivotRot;
        data.networkButtons.Set(PlayerInputButtons.Jump, Input.GetKey(KeyCode.Space));
        return data;
    }
}