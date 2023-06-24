using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UnitBase)), RequireComponent(typeof(UnitMove))]
public class PlayerUnitController : MonoBehaviour
{
    [SerializeField] UnitBase unitBase;
    [SerializeField] UnitMove unitMove;

    [Header("攻击")]
    [SerializeField] UnitMove headMove;
    [SerializeField] float attackDuring = 0.3f;
    [SerializeField] UnitAttacker gunLeft;
    [SerializeField] UnitAttacker gunRight;

    private void OnEnable() {
        unitBase.AddDeathListener(ControlerUnitDeath);
    }

    private void OnDisable() {
        unitBase.RemoveDeathListener(ControlerUnitDeath);
    }

    private void Start() {
        unitMove.SetMoveable(unitBase.stillAlive);
    }

    private void Update() {
        float deltaTime = Time.deltaTime;
        if (unitMove) {
            Movement_Update();
            Rotation_Update();
        }
        if (headMove) {
            Fire_Update(deltaTime);
        }
    }

    //移动操作。
    void Movement_Update() {
        Vector2 move = InputManager.GetHVAxis();
        if (move.sqrMagnitude > 0) {  //转身体
            unitMove.RotateDirection(move);
            unitMove.MoveForward();
        }
        else {
            unitMove.MoveStop();
        }
    }

    //旋转操作。
    void Rotation_Update() {
        //转头
        if (headMove == unitMove && InputManager.GetHVAxis().sqrMagnitude > 0) {
            return;
        }
        headMove.RotateDirection(InputManager.GetMouseWorldPosition() - headMove.selfTransform.position);
    }

    //攻击操作。
    float curAttackDuring;
    bool leftAttack;
    void Fire_Update(float deltaTime) {
        if (curAttackDuring > 0) {
            curAttackDuring -= deltaTime;
        }
        else if (InputManager.GetButton(InputManager.Fire1, true)) {
            if (leftAttack) {
                gunLeft.Fire();
            }
            else {
                gunRight.Fire();
            }
            leftAttack = !leftAttack;
            curAttackDuring += attackDuring;
        }
    }

    void ControlerUnitDeath(UnitBase unit) {
        unitMove.SetMoveable(false);
    }

    /// <summary>
    /// 设置攻击间隔。
    /// </summary>
    public void SetAttackDuring(float during) {
        attackDuring = during;
    }

    public UnitMove GetHeadMove() {
        return headMove;
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
#endif
}
