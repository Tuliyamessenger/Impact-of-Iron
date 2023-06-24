using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemRotate : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    private void OnEnable() {
        ParticleSystem.MainModule mm = particle.main;
        ParticleSystem.MinMaxCurve curve = mm.startRotation;
        curve.constant = -selfTransform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        mm.startRotation = curve;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (particle != null) {
            particle = GetComponent<ParticleSystem>();
        }
    }
#endif
}
