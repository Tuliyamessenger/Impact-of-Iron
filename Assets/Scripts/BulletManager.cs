using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoSingleton<BulletManager>
{
    PrefabPool<BulletBase> bulletPool = new PrefabPool<BulletBase>();

    static public BulletBase DequeueBullet(BulletBase prefab) {
        BulletBase bullet = Instance.bulletPool.DequeueInstance(prefab);
        bullet.SetParent(Instance.selfTransform);
        return bullet;
    }

    static public void EnqueueBullet(BulletBase bullet, BulletBase prefab) {
        Instance.bulletPool.EnqueueInstance(bullet, prefab);
    }

    protected override BulletManager GetSelf() {
        return this;
    }
}