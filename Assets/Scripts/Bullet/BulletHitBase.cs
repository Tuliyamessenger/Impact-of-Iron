using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletHitBase //: MonoBehaviour
{
    [Header("�˺�")]
    [SerializeField] int damage;

    [Header("��͸��")]
    [SerializeField] int strike;

    [Header("�����˺�")]
    [SerializeField] bool targetHit = true;
    [SerializeField] EffectObject hitEffect;

    [Header("��Χ�˺�")]
    [SerializeField] bool areaHit;
    [SerializeField] float areaHitRadius;


    /// <summary> �Ƿ��Ƿ�Χ�˺��� </summary>
    public bool IsAreaHit() { return areaHit; }

    /// <summary> �Ƿ��������˺��� </summary>
    public bool IsTargetHit() { return targetHit; }

    public void HitEffect(Vector2 pos, Vector2 dir) {
        EffectManager em = EffectManager.Instance;
        em.ShowEffect(hitEffect, pos, CommonUtil.Vector2Angle(Vector2.up, dir));
    }

    /// <summary>
    /// �������Ŀ���˺���
    /// </summary>
    public virtual void TakeTargetHit(UnitBase unit) {
        if (targetHit && unit != null) {
            unit.BeAttacked(damage, strike);
        }
    }

    /// <summary>
    /// ��ɷ�Χ�˺���
    /// </summary>
    public virtual void TakeAreaHit() {

    }
}
