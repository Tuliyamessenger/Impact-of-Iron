using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : RecycleObject<EffectObject>
{
    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    private void OnDisable() {
        EffectManager.EnqueueEffect(this, relatePrefab);
    }
}
