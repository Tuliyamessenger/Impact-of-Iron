using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    protected override UnitManager GetSelf() {
        return this;
    }

    #region ̹�˵�λ
    PrefabPool_General<UnitBase> unitPool = new PrefabPool_General<UnitBase>();
    HashSet<UnitBase> friendParty = new HashSet<UnitBase>();
    HashSet<UnitBase> enemyParty = new HashSet<UnitBase>();

    static public UnitBase DequeueUnit(UnitBase prefab) {
        UnitBase unit = Instance.unitPool.DequeueInstance(prefab);
        unit.SetRelatePrefab(prefab);
        unit.selfTransform.SetParent(Instance.selfTransform);
        return unit;
    }

    static public void EnqueueUnit(UnitBase unit, UnitBase prefab) {
        Instance.unitPool.EnqueueInstance(unit, prefab);
    }

    /// <summary>
    /// ������飬ͨ���㼶�жϡ�
    /// </summary>
    static public void RegisterParty(UnitBase unit, int layer) {
        switch (layer) {
            case 7: //�Ѿ�
                Instance.friendParty.Add(unit);
                break;
            case 8: //�о�
                Instance.enemyParty.Add(unit);
                break;
        }
    }

    /// <summary>
    /// �뿪���飬ͨ���㼶�жϡ�
    /// </summary>
    static public void UnregisterParty(UnitBase unit, int layer) {
        switch (layer) {
            case 7: //�Ѿ�
                Instance.friendParty.Remove(unit);
                break;
            case 8: //�о�
                Instance.enemyParty.Remove(unit);
                break;
        }
    }

    /// <summary>
    /// ��ȡ�жԶ��飬ͨ���㼶�жϵжԡ�
    /// </summary>
    static public HashSet<UnitBase> GetOpponentParty(int layer) {
        switch (layer) {
            case 7: //�Ѿ������صо�
                return Instance.enemyParty;
            case 8: //�о��������Ѿ�
                return Instance.friendParty;
        }
        return null;
    }
    #endregion

    #region ��ӡ
    PrefabPool<TankTireTrail> trailPool = new PrefabPool<TankTireTrail>();

    static public TankTireTrail DequeueTrail(TankTireTrail prefab) {
        TankTireTrail trail = Instance.trailPool.DequeueInstance(prefab);
        trail.selfTransform.SetParent(Instance.selfTransform);
        return trail;
    }

    static public void EnqueueTrail(TankTireTrail trail, TankTireTrail prefab) {
        Instance.trailPool.EnqueueInstance(trail, prefab);
    }
    #endregion

    #region ���ϳ���
    PrefabPool<DeathBody> deathPool = new PrefabPool<DeathBody>();
    [SerializeField] DeathBody deathBodyPrefab;

    static public DeathBody DequeueDeathBody() {
        DeathBody body = Instance.deathPool.DequeueInstance(Instance.deathBodyPrefab);
        body.selfTransform.SetParent(Instance.selfTransform);
        return body;
    }

    static public void EnqueueDeathBody(DeathBody body, DeathBody prefab) {
        Instance.deathPool.EnqueueInstance(body, prefab);
    }
#endregion
}
