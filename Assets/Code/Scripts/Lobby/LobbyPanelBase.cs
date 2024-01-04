using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanelBase : MonoBehaviour
{
    [field: SerializeField, Header("Inherited variables")]
    public LobbyPanelType panelType { get; private set; }
    [SerializeField] private Animator panelAnimator;
    protected LobbyUIManager lobbyUIManager;

    public enum LobbyPanelType
    {
        None,
        CreateNicknamePanel,
        MiddleSectionPanel
    }

    public virtual void InitPanel(LobbyUIManager uiManager)
    {
        lobbyUIManager = uiManager;
    }

    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
        const string POP_IN_CLIP_NAME = "In";
        panelAnimator.Play(POP_IN_CLIP_NAME);
    }

    protected void HidePanel()
    {
        const string POP_OUT_CLIP_NAME = "Out";
        StartCoroutine(Utils.PlayAnimSetStateWhenFinished(gameObject, panelAnimator, POP_OUT_CLIP_NAME, false));
    }
}