using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanel : PanelBase
{
    [SerializeField] GameController gameController;

    [Header("°´Å¥")]
    [SerializeField] Button levelShield;
    [SerializeField] Button levelRotate;
    [SerializeField] Button levelBullet;
    [SerializeField] Button levelStrike;

    /// <summary>
    /// 0¶Ü 1×ª 2µ¯ 3´©
    /// </summary>
    public void LevelUpPower(int power) {
        switch (power) {
            case 0:
                levelShield.interactable = false;
                break;
            case 1:
                levelRotate.interactable = false;
                break;
            case 2:
                levelBullet.interactable = false;
                break;
            case 3:
                levelStrike.interactable = false;
                break;
            default:
                return;
        }
        gameController.LevelUpPower(power);
        HidePanel();
    }

    public void ResetLevel() {
        levelShield.interactable = true;
        levelRotate.interactable = true;
        levelBullet.interactable = true;
        levelStrike.interactable = true;
    }
}
