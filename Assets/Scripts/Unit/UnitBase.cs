using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitBase : MonoBehaviour {
    [Header("属性")]
    [SerializeField] int maxHP = 100;
    [SerializeField] int shield = 0;

    [Header("受击")]
    [SerializeField] ColorOverride colorOverride;
    [SerializeField] Transform bodyTrans;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] int explodeLevel = 0;

    public int MaxHP { get { return maxHP; } }
    public int Shield { get { return shield; } }
    public int HP { get; private set; }
    public bool stillAlive { get {  return HP > 0; } }
    public UnitBase relatePrefab { get; private set; }

    Action<UnitBase> deathListener; //死亡时
    Action<int, int, int> HPUpdatedListener; //HP变动时

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    UnitMove _unitMove;
    public UnitMove unitMove {
        get {
            if (_unitMove == null) {
                _unitMove = GetComponent<UnitMove>();
            }
            return _unitMove;
        }
    }

    private void OnEnable() {
        FillHP();
        UnitManager.RegisterParty(this, gameObject.layer);
    }

    private void OnDisable() {
        UnitManager.UnregisterParty(this, gameObject.layer);
        UnitManager.EnqueueUnit(this, relatePrefab); //回收
    }

    /// <summary>
    /// 撞中了什么。
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision) {
        EffectManager em = EffectManager.Instance;
        if (em.AllowCollideEffect()) {
            ContactPoint2D[] contactList = collision.contacts;
            Vector2 pos;
            if (contactList.Length > 1) {
                pos = contactList[0].point;
            }
            else {
                pos = (selfTransform.position + collision.transform.position) / 2;
            }
            em.ShowCollideEffect(pos);
        }
    }

    /// <summary>
    /// 受到攻击。
    /// </summary>
    public virtual void BeAttacked(int damage, int strike) {
        if (!stillAlive) {
            return;
        }
        if (shield > 0) {
            //盾伤害减免。
            float reduce = (float)strike / shield;
            if(reduce > 1) {
                reduce = 1;
            }
            damage = (int)(damage * reduce + 0.5f);
            colorOverride.FlashEffect(reduce);
        }
        else {
            colorOverride.FlashEffect(1);
        }
        if (damage > 0) {
            AddHP(-damage);
        }
    }

    /// <summary>
    /// 增加HP，负数减少。
    /// </summary>
    public void AddHP(int add) {
        HP += add;
        HPUpdatedListener?.Invoke(HP, maxHP, add);
        if (HP > maxHP) {
            HP = maxHP;
        }
        else if(HP < 0) {
            HP = 0;
        }
        if(HP == 0) {
            Death();
        }
    }

    /// <summary>
    /// 单位死亡。
    /// </summary>
    public virtual void Death(bool anime = true) {
        deathListener?.Invoke(this);

        if (anime) {
            //爆炸特效。
            EffectManager.Instance.ShowExplode(explodeLevel, selfTransform.position);

            //跳出报废车身。
            if (bodyTrans && bodySprite) {
                DeathBody db = UnitManager.DequeueDeathBody();
                db.ShowBody(bodySprite.sprite, bodyTrans.position, bodyTrans.rotation, bodyTrans.localScale);
            }
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 添加单位死亡时的监听。
    /// </summary>
    public void AddDeathListener(Action<UnitBase> action) {
        CommonUtil.AddListener(ref deathListener, action);
    }

    /// <summary>
    /// 去除单位死亡时的监听。
    /// </summary>
    public void RemoveDeathListener(Action<UnitBase> action) {
        deathListener -= action;
    }

    /// <summary>
    /// 添加HP变动时的监听。
    /// </summary>
    public void AddHPUpdateListener(Action<int, int, int> action) {
        CommonUtil.AddListener(ref HPUpdatedListener, action);
    }

    /// <summary>
    /// 去除HP变动时的监听。
    /// </summary>
    public void RemoveHPUpdateListener(Action<int, int, int> action) {
        HPUpdatedListener -= action;
    }

    public void SetRelatePrefab(UnitBase prefab) {
        relatePrefab = prefab;
    }

    /// <summary>
    /// 回满HP。
    /// </summary>
    public void FillHP() {
        HP = maxHP;
    }

    public void SetMaxHP(int max) {
        maxHP = max;
        if(HP < maxHP) {
            HP = maxHP;
        }
    }

    public void SetShield(int s) {
        shield = s;
    }
}
