using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTireTrail : RecycleObject<TankTireTrail>
{
    [SerializeField] TrailRenderer trail;

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    /// <summary>
    /// ��ʾ���ۣ��ỹԭ������ʾ�����ݣ�Ȼ��ŵ���λ��ָ���㼶�¡�
    /// </summary>
    public void ShowTrail(UnitBase unit) {
        Transform follower = unit.unitMove.GetTrailTransform();
        if (follower) {
            trail.enabled = false;
            unit.AddDeathListener(UnitDeath);
            selfTransform.SetParent(follower);
            selfTransform.localPosition = Vector3.zero;
            selfTransform.rotation = Quaternion.identity;
            unit.unitMove.SetTrailData(trail);
            gameObject.SetActive(true);
            trail.Clear();
            trail.enabled = true;
        }
    }

    void UnitDeath(UnitBase unit) {
        //����ը�󣬳��ۻ᷵�ع���㼶��Ȼ���Լ���ʧ��
        unit.RemoveDeathListener(UnitDeath);
        selfTransform.SetParent(UnitManager.Instance.selfTransform);
        if(waitCache == null) {
            waitCache = new WaitForSeconds(trail.time);
        }
        StartCoroutine(WaitForDisappear(waitCache));
    }

    IEnumerator WaitForDisappear(WaitForSeconds wait) {
        yield return wait;
        Disappear();
    }
    WaitForSeconds waitCache;

    void Disappear() {
        gameObject.SetActive(false);
        UnitManager.EnqueueTrail(this, GetRelatePrefab());
    }

    /// <summary>
    /// �ֶ������ۡ�
    /// </summary>
    public void Clear() {
        trail.Clear();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if(trail == null) {
            trail = GetComponent<TrailRenderer>();
        }
    }
#endif
}
