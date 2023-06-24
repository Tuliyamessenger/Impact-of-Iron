using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitMove : MonoBehaviour {
    [SerializeField] Rigidbody2D selfRigidbody;

    [Header("移动属性")]
    [SerializeField] int moveSpeed = 10;
    [SerializeField] int rotSpeed = 180;

    [Header("车痕")]
    [SerializeField] TankTireTrail trailPrefab;
    [SerializeField] Transform trailTrans;
    [SerializeField] AnimationCurve trailWidthCurve;
    [SerializeField] Gradient trailFadeColor;

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    UnitBase _unitBase;
    public UnitBase unitBase {
        get {
            if (_unitBase == null) {
                _unitBase = GetComponent<UnitBase>();
            }
            return _unitBase;
        }
    }

    /// <summary>
    /// 可否控制移动。
    /// </summary>
    public bool moveable { get; private set; }

    float moveSpeedFactor = 1;
    TankTireTrail relateTrail;

    private void OnEnable() {
        SetMoveable(true);
        if(trailPrefab && trailTrans) { //显示车痕。
            relateTrail = UnitManager.DequeueTrail(trailPrefab);
            relateTrail.ShowTrail(unitBase);
            StartCoroutine(WaitToClearTrail());
        }
    }

    IEnumerator WaitToClearTrail() {
        yield return waitEnd;
        relateTrail.Clear();
    }
    WaitForEndOfFrame waitEnd = new WaitForEndOfFrame();

    /// <summary>
    /// 按指定方向移动。
    /// </summary>
    public void MoveDirection(Vector2 dir) {
        if (!moveable) {
            return;
        }
        MoveDirection_Inside(dir);
    }

    void MoveDirection_Inside(Vector2 dir) {
        dir.Normalize();
        if (selfRigidbody) {
            selfRigidbody.velocity = dir * moveSpeed * moveSpeedFactor;
        }
    }

    /// <summary>
    /// 向前移动。
    /// </summary>
    public void MoveForward() {
        if (!moveable) {
            return;
        }
        MoveDirection_Inside(selfTransform.up);
    }

    /// <summary>
    /// 停止移动。
    /// </summary>
    public void MoveStop() {
        if (selfRigidbody) {
            selfRigidbody.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 瞬移到指定位置。
    /// </summary>
    public void SetPosition(Vector2 pos) {
        if (!moveable) {
            return;
        }
        SetPosition_Inside(pos);
    }

    public void SetPosition_Inside(Vector2 pos) {
        if (selfRigidbody) {
            selfRigidbody.MovePosition(pos);
        }
        else {
            Vector3 pos3 = selfTransform.position;
            pos3.x = pos.x;
            pos3.y = pos.y;
            selfTransform.position = pos3;
        }
    }

    /// <summary>
    /// 设置可否控制移动。
    /// </summary>
    public void SetMoveable(bool able) {
        moveable = able;
        if (!moveable) {
            MoveDirection_Inside(Vector2.zero);
        }
    }

    /// <summary>
    /// 设置朝向。
    /// </summary>
    public void SetOrientation(Vector2 ori) {
        if (selfRigidbody) {
            selfRigidbody.rotation = CommonUtil.Vector2Angle(Vector2.up, ori);
        }
        else {
            selfTransform.rotation = Quaternion.Euler(0, 0, CommonUtil.Vector2Angle(Vector2.up, ori));
        }
    }

    /// <summary>
    /// 按指定方向旋转。
    /// </summary>
    public void RotateDirection(Vector2 dir) {
        //float angle = CommonUtil.Vector2Angle(selfTransform.up, dir); //误差有点大。
        float rotateZ = selfRigidbody ? selfRigidbody.rotation : selfTransform.rotation.eulerAngles.z;
        float angle = CommonUtil.Vector2Angle(Vector2.up, dir) - rotateZ;
        if(angle > 180) {
            angle -= 360;
        }
        if(angle < -180) {
            angle += 360;
        }
        if (Mathf.Abs(angle) <= 0.1f) {
            SetOrientation(dir);
        }
        else {
            float rot = rotSpeed * (angle < 0 ? -1 : 1) * Time.deltaTime;
            if (Mathf.Abs(rot) >= Mathf.Abs(angle)) { //不要转超过了。
                rot = angle;
            }
            if (selfRigidbody) {
                selfRigidbody.SetRotation(selfRigidbody.rotation + rot);
            }
            else {
                selfTransform.rotation = Quaternion.Euler(0, 0, rotateZ + rot);
            }
        }
    }

    /// <summary>
    /// 获得移动速度。
    /// </summary>
    public float GetMoveSpeed() {
        return moveSpeed;
    }

    /// <summary>
    /// 获得旋转速度。
    /// </summary>
    public float GetRotateSpeed() {
        return rotSpeed;
    }

    /// <summary>
    /// 设置移动速度。
    /// </summary>
    public void SetMoveSpeed(int speed) {
        moveSpeed = speed;
    }

    /// <summary>
    /// 设置旋转速度。
    /// </summary>
    public void SetRotateSpeed(int speed) {
        rotSpeed = speed;
    }

    /// <summary>
    /// 设置速度倍率。
    /// </summary>
    public void SetMoveSpeedFactor(float factor) {
        moveSpeedFactor = factor;
    }

    public Transform GetTrailTransform() {
        return trailTrans;
    }

    /// <summary>
    /// 传入TrailRenderer，会自动改变值。
    /// </summary>
    public void SetTrailData(TrailRenderer trail) {
        trail.widthCurve = trailWidthCurve;
        trail.colorGradient = trailFadeColor;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (!selfRigidbody) {
            selfRigidbody = GetComponent<Rigidbody2D>();
        }
    }
#endif
}