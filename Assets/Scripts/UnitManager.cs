using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    protected override UnitManager GetSelf() {
        return this;
    }

    #region 坦克单位
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
    /// 加入队伍，通过层级判断。
    /// </summary>
    static public void RegisterParty(UnitBase unit, int layer) {
        switch (layer) {
            case 7: //友军
                Instance.friendParty.Add(unit);
                break;
            case 8: //敌军
                Instance.enemyParty.Add(unit);
                break;
        }
    }

    /// <summary>
    /// 离开队伍，通过层级判断。
    /// </summary>
    static public void UnregisterParty(UnitBase unit, int layer) {
        switch (layer) {
            case 7: //友军
                Instance.friendParty.Remove(unit);
                break;
            case 8: //敌军
                Instance.enemyParty.Remove(unit);
                break;
        }
    }

    /// <summary>
    /// 获取敌对队伍，通过层级判断敌对。
    /// </summary>
    static public HashSet<UnitBase> GetOpponentParty(int layer) {
        switch (layer) {
            case 7: //友军，返回敌军
                return Instance.enemyParty;
            case 8: //敌军，返回友军
                return Instance.friendParty;
        }
        return null;
    }
    #endregion

    #region 车印
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

    #region 报废车身
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
