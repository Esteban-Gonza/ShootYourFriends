using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateNicknamePanel : LobbyPanelBase
{
    [Header("Non-inherited variables")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button createNicknameBtn;

    private const int MIN_CHAR_NICKNAME = 2;

    public override void InitPanel(LobbyUIManager lobbyUIManager)
    {
        base.InitPanel(lobbyUIManager);

        createNicknameBtn.interactable = false;
        createNicknameBtn.onClick.AddListener(OnClickCreateNickname);
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string arg0)
    {
        createNicknameBtn.interactable = arg0.Length >= MIN_CHAR_NICKNAME; 
    }

    private void OnClickCreateNickname()
    {
        var nickname = inputField.text;
        if(nickname.Length >= MIN_CHAR_NICKNAME)
        {
            base.HidePanel();
            lobbyUIManager.ShowPanel(LobbyPanelType.MiddleSectionPanel);
        }
    }
}