using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitBase : MonoBehaviour {
    [Header("����")]
    [SerializeField] int maxHP = 100;
    [SerializeField] int shield = 0;

    [Header("�ܻ�")]
    [SerializeField] ColorOverride colorOverride;
    [SerializeField] Transform bodyTrans;
    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] int explodeLevel = 0;

    public int MaxHP { get { return maxHP; } }
    public int Shield { get { return shield; } }
    public int HP { get; private set; }
    public bool stillAlive { get {  return HP > 0; } }
    public UnitBase relatePrefab { get; private set; }

    Action<UnitBase> deathListener; //����ʱ
    Action<int, int, int> HPUpdatedListener; //HP�䶯ʱ

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
        UnitManager.EnqueueUnit(this, relatePrefab); //����
    }

    /// <summary>
    /// ײ����ʲô��
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
    /// �ܵ�������
    /// </summary>
    public virtual void BeAttacked(int damage, int strike) {
        if (!stillAlive) {
            return;
        }
        if (shield > 0) {
            //���˺����⡣
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
    /// ����HP���������١�
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
    /// ��λ������
    /// </summary>
    public virtual void Death(bool anime = true) {
        deathListener?.Invoke(this);

        if (anime) {
            //��ը��Ч��
            EffectManager.Instance.ShowExplode(explodeLevel, selfTransform.position);

            //�������ϳ���
            if (bodyTrans && bodySprite) {
                DeathBody db = UnitManager.DequeueDeathBody();
                db.ShowBody(bodySprite.sprite, bodyTrans.position, bodyTrans.rotation, bodyTrans.localScale);
            }
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// ��ӵ�λ����ʱ�ļ�����
    /// </summary>
    public void AddDeathListener(Action<UnitBase> action) {
        CommonUtil.AddListener(ref deathListener, action);
    }

    /// <summary>
    /// ȥ����λ����ʱ�ļ�����
    /// </summary>
    public void RemoveDeathListener(Action<UnitBase> action) {
        deathListener -= action;
    }

    /// <summary>
    /// ���HP�䶯ʱ�ļ�����
    /// </summary>
    public void AddHPUpdateListener(Action<int, int, int> action) {
        CommonUtil.AddListener(ref HPUpdatedListener, action);
    }

    /// <summary>
    /// ȥ��HP�䶯ʱ�ļ�����
    /// </summary>
    public void RemoveHPUpdateListener(Action<int, int, int> action) {
        HPUpdatedListener -= action;
    }

    public void SetRelatePrefab(UnitBase prefab) {
        relatePrefab = prefab;
    }

    /// <summary>
    /// ����HP��
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
