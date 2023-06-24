using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttacker : MonoBehaviour
{
    [Header("�ӵ�Ԥ����")]
    public BulletBase bulletPrefab;

    [Header("ǹ��λ��")]
    public Transform muzzle;

    [Header("�޸��ӵ�����")]
    public bool customBullet;
    public BulletHitBase customData;

    [SerializeField] Animation fireAnimation;
    [SerializeField] Rigidbody2D initVelocity_body;

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Fire() {
        BulletBase bullet = BulletManager.DequeueBullet(bulletPrefab);
        if (customBullet) {
            bullet.SetHitData(customData);
        }
        if (initVelocity_body) {
            bullet.Fire(muzzle.position, (Vector2)muzzle.up, initVelocity_body.velocity);
        }
        else {
            bullet.Fire(muzzle.position, (Vector2)muzzle.up);
        }
        if (fireAnimation) {
            fireAnimation.Stop();
            fireAnimation.Play();
        }
    }

    private void OnEnable() {
        //��ԭ����λ�á�
        AnimationState state = fireAnimation[fireAnimation.clip.name];
        state.time = state.length;
        fireAnimation.Play();
    }
}
