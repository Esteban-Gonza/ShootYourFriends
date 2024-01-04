using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class MiddleSectionPanel : LobbyPanelBase
{
    [Header("Non-inherited variables")]
    [SerializeField] private Button joinRandomRoomBtn;
    [SerializeField] private Button joinRoomByArgBtn;
    [SerializeField] private Button createRoomgBtn;

    [SerializeField] private TMP_InputField joinRoomByArgInputField;
    [SerializeField] private TMP_InputField createRoomInputField;

    private NetworkRunnerController networkRunnerController;

    public override void InitPanel(LobbyUIManager uiManager)
    {
        base.InitPanel(uiManager);

        networkRunnerController = GlobalManagers.Instance.networkRunnerController;
        joinRandomRoomBtn.onClick.AddListener(JoinRandomRoom);
        joinRoomByArgBtn.onClick.AddListener(call: () => CreateRoom(GameMode.Client, joinRoomByArgInputField.text));
        createRoomgBtn.onClick.AddListener(call:() => CreateRoom(GameMode.Host, createRoomInputField.text));

    }

    private void CreateRoom(GameMode mode, string field)
    {
        if (field.Length >= 2)
        {
            Debug.Log($"---------{mode}---------");
            GlobalManagers.Instance.networkRunnerController.StartGame(mode, field);
        }
    }

    private void JoinRandomRoom()
    {
        Debug.Log($"---------JoinRandomRoom---------");
        networkRunnerController.StartGame(GameMode.AutoHostOrClient, string.Empty);
    }
}