using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData : INetworkInput
{
    public float horizontalInput;
    public NetworkButtons networkButtons;
}