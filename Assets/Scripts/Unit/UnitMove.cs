using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UnitMove : MonoBehaviour {
    [SerializeField] Rigidbody2D selfRigidbody;

    [Header("�ƶ�����")]
    [SerializeField] int moveSpeed = 10;
    [SerializeField] int rotSpeed = 180;

    [Header("����")]
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
    /// �ɷ�����ƶ���
    /// </summary>
    public bool moveable { get; private set; }

    float moveSpeedFactor = 1;
    TankTireTrail relateTrail;

    private void OnEnable() {
        SetMoveable(true);
        if(trailPrefab && trailTrans) { //��ʾ���ۡ�
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
    /// ��ָ�������ƶ���
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
    /// ��ǰ�ƶ���
    /// </summary>
    public void MoveForward() {
        if (!moveable) {
            return;
        }
        MoveDirection_Inside(selfTransform.up);
    }

    /// <summary>
    /// ֹͣ�ƶ���
    /// </summary>
    public void MoveStop() {
        if (selfRigidbody) {
            selfRigidbody.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// ˲�Ƶ�ָ��λ�á�
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
    /// ���ÿɷ�����ƶ���
    /// </summary>
    public void SetMoveable(bool able) {
        moveable = able;
        if (!moveable) {
            MoveDirection_Inside(Vector2.zero);
        }
    }

    /// <summary>
    /// ���ó���
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
    /// ��ָ��������ת��
    /// </summary>
    public void RotateDirection(Vector2 dir) {
        //float angle = CommonUtil.Vector2Angle(selfTransform.up, dir); //����е��
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
            if (Mathf.Abs(rot) >= Mathf.Abs(angle)) { //��Ҫת�����ˡ�
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
    /// ����ƶ��ٶȡ�
    /// </summary>
    public float GetMoveSpeed() {
        return moveSpeed;
    }

    /// <summary>
    /// �����ת�ٶȡ�
    /// </summary>
    public float GetRotateSpeed() {
        return rotSpeed;
    }

    /// <summary>
    /// �����ƶ��ٶȡ�
    /// </summary>
    public void SetMoveSpeed(int speed) {
        moveSpeed = speed;
    }

    /// <summary>
    /// ������ת�ٶȡ�
    /// </summary>
    public void SetRotateSpeed(int speed) {
        rotSpeed = speed;
    }

    /// <summary>
    /// �����ٶȱ��ʡ�
    /// </summary>
    public void SetMoveSpeedFactor(float factor) {
        moveSpeedFactor = factor;
    }

    public Transform GetTrailTransform() {
        return trailTrans;
    }

    /// <summary>
    /// ����TrailRenderer�����Զ��ı�ֵ��
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