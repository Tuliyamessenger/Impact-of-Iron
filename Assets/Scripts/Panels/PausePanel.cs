using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PausePanel : PanelBase
{
    [SerializeField] GameController gameController;
    [SerializeField] TextMeshProUGUI resumeText;
    [SerializeField] string resumeTxt = "������Ϸ";
    [SerializeField] string restartTxt = "���¿�ʼ";

    public override void ShowPanel() {
        base.ShowPanel();

        resumeText.text = gameController.IsGameOver() ? restartTxt : resumeTxt;
    }

    public void ResumeGame() {
        HidePanel();
        if (gameController.IsGameOver()) {
            gameController.RestartGame();
        }
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
