using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletHitBase //: MonoBehaviour
{
    [Header("…À∫¶")]
    [SerializeField] int damage;

    [Header("¥©Õ∏¡¶")]
    [SerializeField] int strike;

    [Header("√¸÷–…À∫¶")]
    [SerializeField] bool targetHit = true;
    [SerializeField] EffectObject hitEffect;

    [Header("∑∂Œß…À∫¶")]
    [SerializeField] bool areaHit;
    [SerializeField] float areaHitRadius;


    /// <summary>  «∑Ò «∑∂Œß…À∫¶°£ </summary>
    public bool IsAreaHit() { return areaHit; }

    /// <summary>  «∑Ò «√¸÷–…À∫¶°£ </summary>
    public bool IsTargetHit() { return targetHit; }

    public void HitEffect(Vector2 pos, Vector2 dir) {
        EffectManager em = EffectManager.Instance;
        em.ShowEffect(hitEffect, pos, CommonUtil.Vector2Angle(Vector2.up, dir));
    }

    /// <summary>
    /// ‘Ï≥…√¸÷–ƒø±Í…À∫¶°£
    /// </summary>
    public virtual void TakeTargetHit(UnitBase unit) {
        if (targetHit && unit != null) {
            unit.BeAttacked(damage, strike);
        }
    }

    /// <summary>
    /// ‘Ï≥…∑∂Œß…À∫¶°£
    /// </summary>
    public virtual void TakeAreaHit() {

    }
}
