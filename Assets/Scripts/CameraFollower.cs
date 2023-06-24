using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform follower;
    [SerializeField] float seeAimFactor = 0.1f;

    Transform _transform;
    public Transform selfTransform {
        get {
            if (!_transform) {
                _transform = transform;
            }
            return _transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (follower != null) {
            Vector3 pos = selfTransform.position;
            Vector2 aimPos = InputManager.GetMouseWorldPosition();
            Vector2 fPos = Vector2.Lerp(follower.position, aimPos, seeAimFactor);
            pos.x = fPos.x;
            pos.y = fPos.y;
            selfTransform.position = pos;
        }
    }
}
