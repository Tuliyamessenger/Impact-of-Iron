using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject<T> : MonoBehaviour where T : Component
{
    protected T relatePrefab;

    public T GetRelatePrefab() { return relatePrefab; }
    public void SetRelatePrefab(T prefab) { relatePrefab = prefab; }
}
