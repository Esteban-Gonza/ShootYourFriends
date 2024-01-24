using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour, IBeforeUpdate
{
    public Quaternion localQuaternionPivotRot { get; set; }

    [SerializeField] private Camera localCam;
    [SerializeField] private Transform pivotToRotate;

    [Networked] private Quaternion currentPlayerPivotRotation { get; set; }

    public void BeforeUpdate()
    {
        if(Runner.LocalPlayer == Object.HasInputAuthority)
        {
            var direction = localCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            localQuaternionPivotRot = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input))
        {
            currentPlayerPivotRotation = input.gunPivotRotation;
        }

        pivotToRotate.rotation = currentPlayerPivotRotation;
    }
}