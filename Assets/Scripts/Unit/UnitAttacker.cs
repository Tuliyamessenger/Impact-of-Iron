using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttacker : MonoBehaviour
{
    [Header("子弹预制体")]
    public BulletBase bulletPrefab;

    [Header("枪口位置")]
    public Transform muzzle;

    [Header("修改子弹属性")]
    public bool customBullet;
    public BulletHitBase customData;

    [SerializeField] Animation fireAnimation;
    [SerializeField] Rigidbody2D initVelocity_body;

    /// <summary>
    /// 开火。
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
        //还原动画位置。
        AnimationState state = fireAnimation[fireAnimation.clip.name];
        state.time = state.length;
        fireAnimation.Play();
    }
}
