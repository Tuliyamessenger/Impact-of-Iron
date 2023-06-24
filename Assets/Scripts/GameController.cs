using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] float minShowEnemyTime = 1;
    [SerializeField] float enemyShowDistance = 45;
    [SerializeField] int startAllowEnemyPower = 10;

    [Header("���")]
    [SerializeField] UnitBase playerUnit;
    [SerializeField] PlayerUnitController playerController;
    [SerializeField] int HPRecoverSec = 1;
    [SerializeField] int[] levelScore;
    [SerializeField] Vector2Int levelHP;
    [SerializeField] Vector2Int levelShield;
    [SerializeField] Vector2Int levelRotate;
    [SerializeField] Vector2Int levelRotateHead;
    [SerializeField] Vector2 levelBullet;
    [SerializeField] BulletBase levetStrick_Before;
    [SerializeField] BulletBase levetStrick_After;
    [SerializeField] UnitAttacker attacker_L;
    [SerializeField] UnitAttacker attacker_R;

    [Header("����")]
    [SerializeField] UnitBase[] enemyPrefabList;
    [SerializeField] int[] enemyPower;

    [Header("����")]
    [SerializeField] FillBar HPBar;
    [SerializeField] FillBar ScoreBar;
    [SerializeField] LevelUpPanel levelUpPanel;
    [SerializeField] PausePanel pausePanel;

    Dictionary<UnitBase, int> enemyPowerDic;

    int curEnemyPower; //��ǰ���˾�����
    int curScore; //��ǰ��������������˾�����

    float curShowEnemyDuring;
    float curRecoverTime = 1;
    int curLevel = 0;

    private void Awake() {
        curEnemyPower = 0;
        enemyPowerDic = new Dictionary<UnitBase, int>();
        for(int i = 0; i < enemyPrefabList.Length; i += 1) {
            enemyPowerDic.Add(enemyPrefabList[i], enemyPower[i]);
        }

        playerUnit.AddHPUpdateListener(PlayerHPUpdated);
    }

    private void Update() {
        float deltaTime = Time.deltaTime;
        curShowEnemyDuring += deltaTime;
        //һֱ��ӵ��˵���ǰ������
        if (curShowEnemyDuring > minShowEnemyTime && curEnemyPower < GetAllowEnemyPower()) {
            minShowEnemyTime = 0;
            ShowAnEnemy();
        }

        //��Ȼ��Ѫ��
        if (playerUnit.stillAlive) {
            curRecoverTime -= deltaTime;
            if (curRecoverTime <= 0) {
                curRecoverTime += 1;
                playerUnit.AddHP(HPRecoverSec);
            }
        }

        //��ͣ�˵�
        if(InputManager.GetButton(InputManager.Cancel)) {
            if (pausePanel.isOpen) {
                pausePanel.HidePanel();
            }
            else {
                pausePanel.ShowPanel();
            }
        }
    }

    private void Start() {
        RestartGame();
    }

    //���¿�ʼ��Ϸ���ݻ����е��ˣ��ظ���ҡ�
    public void RestartGame() {
        //��ϴ���ˡ�
        HashSet<UnitBase> enemyAll = UnitManager.GetOpponentParty(7);
        if (enemyAll.Count > 0) {
            List<UnitBase> enemyList = new List<UnitBase>(enemyAll.Count);
            foreach (UnitBase unit in enemyAll) {
                enemyList.Add(unit);
            }
            for(int i = 0; i < enemyList.Count; i += 1) {
                enemyList[i].Death(false);
            }
        }

        //������ҡ�
        playerUnit.selfTransform.position = Vector3.zero;
        playerUnit.FillHP();
        ResetPlayerUnitState();

        //������Ϸ���ȡ�
        curScore = 0;
        curLevel = 0;
        curEnemyPower = 0;

        PlayerHPUpdated(playerUnit.HP, playerUnit.MaxHP, 0);
        ScoreUpdated();
        playerUnit.gameObject.SetActive(true);
    }

    //����һ�����ˡ�
    void ShowAnEnemy() {
        int id = Random.Range(0, enemyPrefabList.Length - (GetAllowEnemyPower() < 20 ? 1 : 0)); //20����ǰ������̹��
        UnitBase enemy = UnitManager.DequeueUnit(enemyPrefabList[id]);
        enemy.FillHP();
        //����Ļ�����ɡ�
        Vector2 randomNormal = Random.insideUnitCircle.normalized + (Vector2)playerUnit.selfTransform.up; //������ǰ��Բ׶�������ɡ�
        enemy.selfTransform.position = (Vector2)playerUnit.selfTransform.position + randomNormal.normalized * enemyShowDistance; 
        enemy.gameObject.gameObject.SetActive(true);
        enemy.AddDeathListener(EnemyDeath);
        curEnemyPower += enemyPower[id];
    }

    void EnemyDeath(UnitBase unit) {
        unit.RemoveDeathListener(EnemyDeath);
        if (enemyPowerDic.TryGetValue(unit.relatePrefab, out int power)) {
            curEnemyPower -= power;
            curScore += power;
            ScoreUpdated();
        }
    }

    int GetAllowEnemyPower() {
        return curScore / 2 + startAllowEnemyPower;
    }

    void PlayerHPUpdated(int curHP, int maxHP, int add) {
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        HPBar.SetFill((float)curHP / maxHP);
        HPBar.SetText(curHP.ToString());
    }

    void ScoreUpdated() {
        if(curLevel >= levelScore.Length) {
            ScoreBar.SetFill(1);
        }
        else {
            int lastScore = curLevel > 0 ? levelScore[curLevel - 1] : 0;
            ScoreBar.SetFill((float)(curScore - lastScore) / (levelScore[curLevel] - lastScore));
            if(curScore >= levelScore[curLevel]) {
                ShowLevelUpPanel();//��ʾ�������档
            }
        }
        ScoreBar.SetText(curScore.ToString());
    }

    void ShowLevelUpPanel() {
        if (!IsGameOver()) {
            levelUpPanel.ShowPanel();
        }
    }

    /// <summary>
    /// 0�� 1ת 2�� 3��
    /// </summary>
    public void LevelUpPower(int power) {
        switch (power) {
            case 0:
                playerUnit.SetMaxHP(levelHP.y);
                playerUnit.SetShield(levelShield.y);
                playerUnit.FillHP();
                break;
            case 1:
                playerUnit.unitMove.SetRotateSpeed(levelRotate.y);
                playerController.GetHeadMove().SetRotateSpeed(levelRotateHead.y);
                break;
            case 2:
                playerController.SetAttackDuring(levelBullet.y);
                break;
            case 3:
                attacker_L.bulletPrefab = levetStrick_After;
                attacker_R.bulletPrefab = levetStrick_After;
                break;
        }
        curLevel += 1;
        ScoreUpdated();
    }

    /// <summary>
    /// ����������ԡ�
    /// </summary>
    public void ResetPlayerUnitState() {
        playerUnit.SetMaxHP(levelHP.x);
        playerUnit.SetShield(levelShield.x);

        playerUnit.unitMove.SetRotateSpeed(levelRotate.x);
        playerController.GetHeadMove().SetRotateSpeed(levelRotateHead.x);

        playerController.SetAttackDuring(levelBullet.x);

        attacker_L.bulletPrefab = levetStrick_Before;
        attacker_R.bulletPrefab = levetStrick_Before;

        //��ԭ�������档
        levelUpPanel.ResetLevel();
    }

    public bool IsGameOver() {
        return !playerUnit.stillAlive;
    }
}
