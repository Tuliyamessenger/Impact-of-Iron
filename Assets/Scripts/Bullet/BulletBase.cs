using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : RecycleObject<BulletBase>
{
    [SerializeField] BulletHitBase hitData;
    [SerializeField] Rigidbody2D selfRigidbody;
    [SerializeField] float moveSpeed = 10;

    GameObject _gameObject;
    public GameObject selfGameObject {
        get {
            if(_gameObject == null) {
                _gameObject = gameObject;
            }
            return _gameObject;
        }
    }

    Transform _transform;
    public Transform selfTransform {
        get {
            if (_transform == null) {
                _transform = transform;
            }
            return _transform;
        }
    }

    public float lifeTime { get; private set; }

    private void Update() {
        LifeTime_Update(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //������Ч
        hitData.HitEffect(collision.ClosestPoint(selfTransform.position), selfTransform.up);

        //��������
        if (hitData.IsTargetHit() && collision.TryGetComponent<UnitBase>(out UnitBase unit)) {
            hitData.TakeTargetHit(unit);
        }
        Explode();
    }

    /// <summary>
    /// �����ֻ�ӵ���
    /// </summary>
    public void Fire(Vector2 pos, Vector2 dir) {
        Fire(pos, dir, Vector2.zero);
    }

    /// <summary>
    /// �����ֻ�ӵ���
    /// </summary>
    public void Fire(Vector2 pos, Vector2 dir, Vector2 initVelocity, float life = 3) {
        lifeTime = life;
        selfTransform.position = pos;
        selfTransform.rotation = Quaternion.Euler(0, 0, CommonUtil.Vector2Angle(Vector2.up, dir));

        selfGameObject.SetActive(true);
        selfRigidbody.velocity = dir * moveSpeed + initVelocity;
    }

    /// <summary>
    /// �ӵ����ұ�ը��
    /// </summary>
    public void Explode() {
        //��ըAOE
        if (hitData.IsAreaHit()) {
            hitData.TakeAreaHit();
        }
        BulletRecycle();
    }

    //�ӵ�����ʱ������á�
    void LifeTime_Update(float deltaTime) {
        lifeTime -= deltaTime;
        if(lifeTime <= 0) {
            Explode();
        }
    }

    public void SetBulletSpeed(float speed) {
        moveSpeed = speed;
    }

    public void SetParent(Transform parent) {
        selfTransform.SetParent(parent);
    }

    //�����ӵ����Żع���Ԥ������
    void BulletRecycle() {
        gameObject.SetActive(false);
        BulletManager.EnqueueBullet(this, relatePrefab);
    }

    /// <summary>
    /// �޸��������ݡ�
    /// </summary>
    public void SetHitData(BulletHitBase data) {
        hitData = data;
    }

    public BulletHitBase GetHitData() {
        return hitData;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (!selfRigidbody) {
            selfRigidbody = GetComponent<Rigidbody2D>();
            if (selfRigidbody && TryGetComponent<Collider2D>(out Collider2D collider)) {
                collider.isTrigger = true;
            }
        }
    }
#endif
}
