using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UnitBase)), RequireComponent(typeof(UnitMove))]
public class AIUnitControler : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] UnitMove unitMove;

    [Header("攻击")]
    [SerializeField] UnitMove headMove;
    [SerializeField] float attackDuring = 0.3f; //开火间隔
    [SerializeField] float attackKeeping = 1.2f; //攻击持续时间
    [SerializeField] float attackWait = 3.6f; //攻击后等待时间
    [SerializeField] float attackWaitRandom = 0.6f; //攻击等待时间随机增减半径
    [SerializeField] float attackRadius = 10; //到达半径内就开火
    [SerializeField] UnitAttacker[] gunList;

    [Header("移动")]
    [SerializeField] float targetPosRadius = 10; //移动到达的半径。

    UnitBase targetUnit;
    Vector2 targetPos;
    float closeRadius = 1; //目标位置这个半径内算在附近。

    int _selfLayer = -1;
    public int selfLayer {
        get {
            if(_selfLayer < 0) {
                _selfLayer = gameObject.layer;
            }
            return _selfLayer;
        }
    }

    private void OnEnable() {
        unitBase.AddDeathListener(ControlerUnitDeath);
        refreshTargetPos = true;
        stopUpdate = false;
    }

    private void OnDisable() {
        unitBase.RemoveDeathListener(ControlerUnitDeath);
    }

    private void Start() {
        //半径 = 线速度 / 角速度
        closeRadius = unitMove.GetMoveSpeed() * 100 / unitMove.GetRotateSpeed();
        unitMove.SetMoveable(unitBase.stillAlive);
    }

    bool refreshTargetPos = true;
    bool stopUpdate = false;
    private void Update() {
        if (stopUpdate) {
            return;
        }
        if (!IsTargetAvailable()) {
            FindTargetUnit();
            if (!IsTargetAvailable()) {
                stopUpdate = true;
                return;
            }
        }

        float deltaTime = Time.deltaTime;
        Vector2 dis = targetUnit.selfTransform.position - unitMove.selfTransform.position;

        if (unitMove) {
            if (refreshTargetPos) {
                refreshTargetPos = false;
                RefreshTargetPosition();
            }
            Vector2 movedir = targetPos - (Vector2)unitMove.selfTransform.position;
            

            if (unitMove == headMove) {
                if (movedir.sqrMagnitude > closeRadius * closeRadius / 2f) {
                    Movement_Update(movedir);
                }
                else {
                    unitMove.MoveStop();
                    Rotation_Update(dis);
                }
            }
            else {
                Movement_Update(movedir);
                Rotation_Update(dis);
            }

            //在位置附近，但是距离目标很远就刷新。
            if (movedir.sqrMagnitude < closeRadius * closeRadius && dis.sqrMagnitude > targetPosRadius * targetPosRadius) {
                refreshTargetPos = true;
            }
        }

        if (headMove) {
            Fire_Update(deltaTime, dis);
        }
    }

    //移动操作。
    void Movement_Update(Vector2 dis) {
        if (!targetUnit) {
            return;
        }

        float factor = 0;
        if(closeRadius > 0) {
            factor = dis.magnitude / closeRadius;
        }
        if (factor >= 1) {  //转身体
            unitMove.SetMoveSpeedFactor(1);
        }
        else {
            unitMove.SetMoveSpeedFactor(factor);
        }
        unitMove.RotateDirection(dis);
        unitMove.MoveForward();
    }

    //旋转操作。
    void Rotation_Update(Vector2 dis) {
        if (!targetUnit) {
            return;
        }
        //转头
        headMove.RotateDirection(dis);
    }

    //攻击操作。
    float curAttackDuring;
    int attackGunIndex;
    float curAttackWait;
    void Fire_Update(float deltaTime, Vector2 dis) {
        //瞄准后再发射。
        if (curAttackWait < attackKeeping || Mathf.Abs(CommonUtil.Vector2Angle(headMove.selfTransform.up, dis)) <= 10) {
            curAttackWait -= deltaTime;
        }
        if(curAttackWait <= 0) {
            curAttackWait = attackWait + attackKeeping + Random.Range(-attackWaitRandom, attackWaitRandom);
        }
        if (curAttackDuring > 0) {
            curAttackDuring -= deltaTime;
        }
        else if (curAttackWait <= attackKeeping && dis.sqrMagnitude <= attackRadius * attackRadius) {
            if(attackGunIndex < 0) {
                attackGunIndex = gunList.Length - 1;
            }
            else if(attackGunIndex >= gunList.Length){
                attackGunIndex = 0;
            }
            gunList[attackGunIndex].Fire();
            attackGunIndex += 1;
            curAttackDuring += attackDuring;
            if(curAttackDuring < 0) {
                curAttackDuring = 0;
            }
        }
    }

    void ControlerUnitDeath(UnitBase unit) {
        unitMove.SetMoveable(false);
    }

    //寻找并确定目标。
    void FindTargetUnit() {
        HashSet<UnitBase> unitParty = UnitManager.GetOpponentParty(selfLayer);

        //寻找最近的目标。
        UnitBase find = null;
        float dis = 0;
        foreach (UnitBase unit in unitParty) {
            if (unit.stillAlive) {
                float tempDis = (unitBase.selfTransform.position - unit.selfTransform.position).sqrMagnitude;
                if (find == null || dis > tempDis) {
                    find = unit;
                    dis = tempDis;
                }
            }
        }

        targetUnit = find;

        RefreshTargetPosition();
    }

    //刷新要去的位置，在目标的指定半径的范围内。
    void RefreshTargetPosition() {
        if (IsTargetAvailable()) {
            //越外层几率越高。
            float rate = Random.Range(0, 1);
            targetPos = (1 - rate * rate) * targetPosRadius * Random.insideUnitCircle.normalized + (Vector2)targetUnit.selfTransform.position;
        }
    }

    bool IsTargetAvailable() {
        return targetUnit != null && targetUnit.stillAlive;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (!unitBase) {
            unitBase = GetComponent<UnitBase>();
        }
        if (!unitMove) {
            unitMove = GetComponent<UnitMove>();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(unitBase.selfTransform.position, targetPos);
        Gizmos.DrawWireSphere(targetPos, closeRadius);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(unitBase.selfTransform.position, targetPosRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(unitBase.selfTransform.position, attackRadius);
    }
#endif
}
