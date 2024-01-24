using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSpwanerController : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerNetworkPrefab;
    [SerializeField] private Transform[] spawnPoints;

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            foreach(var item in Runner.ActivePlayers)
            {
                SpawnPlayer(item);
            }
        }
    }

    private void SpawnPlayer(PlayerRef playerRef)
    {
        if (Runner.IsServer)
        {
            var index = playerRef % spawnPoints.Length;
            var spawnPoint = spawnPoints[index].transform.position;
            var playerObj = Runner.Spawn(playerNetworkPrefab, spawnPoint, Quaternion.identity, playerRef);

            Runner.SetPlayerObject(playerRef, playerObj);
        }
    }

    private void DespawnPlayer(PlayerRef playerRef)
    {
        if(Runner.IsServer)
        {
            if(Runner.TryGetPlayerObject(playerRef, out var playerNetwornObject))
            {
                Runner.Despawn(playerNetwornObject);
            }

            //Reset player object
            Runner.SetPlayerObject(playerRef, null);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnPlayer(player);
    }
}