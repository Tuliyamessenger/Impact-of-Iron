using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    public EffectObject collideEffectPrefab;
    float collideEffectCold;

    public EffectObject hitEffect_S_Prefab;
    public EffectObject hitEffect_M_Prefab;
    public EffectObject explode_S_Prefab;
    public EffectObject explode_M_Prefab;

    protected override EffectManager GetSelf() {
        return this;
    }

    PrefabPool<EffectObject> effectPool = new PrefabPool<EffectObject>();

    static public EffectObject DequeueEffect(EffectObject prefab) {
        EffectObject effect = Instance.effectPool.DequeueInstance(prefab);
        effect.selfTransform.SetParent(Instance.selfTransform);
        return effect;
    }

    static public void EnqueueEffect(EffectObject effect, EffectObject prefab) {
        Instance.effectPool.EnqueueInstance(effect, prefab);
    }

    private void Update() {
        float deltaTime = Time.deltaTime;
        if(collideEffectCold > 0) {
            collideEffectCold -= deltaTime;
        }
    }

    /// <summary>
    /// 当前可否显示撞击花火。
    /// </summary>
    public bool AllowCollideEffect() {
        return collideEffectCold <= 0;
    }

    /// <summary>
    /// 显示撞击花火，自带冷却。
    /// </summary>
    public void ShowCollideEffect(Vector2 pos) {
        if (collideEffectCold <= 0) {
            ShowEffect(collideEffectPrefab, pos, 0);
            collideEffectCold = 0.2f;
        }
    }

    /// <summary>
    /// 通用的特效显示。
    /// </summary>
    public EffectObject ShowEffect(EffectObject prefab, Vector2 pos, float rot) {
        EffectObject eo = DequeueEffect(prefab);
        eo.selfTransform.position = pos;
        eo.selfTransform.rotation = Quaternion.Euler(0, 0, rot);
        eo.gameObject.SetActive(true);
        return eo;
    }

    /// <summary>
    /// 爆炸的特效显示,level 0=S 1=M
    /// </summary>
    public void ShowExplode(int level, Vector2 pos) {
        EffectObject prefab = null;
        switch (level) {
            case 0:
                prefab = explode_S_Prefab;
                break;
            case 1:
                prefab = explode_M_Prefab;
                break;
        }
        if(prefab != null) {
            ShowEffect(prefab, pos, 0);
        }
    }
}