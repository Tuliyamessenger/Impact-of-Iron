using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool<T> where T : RecycleObject<T> {
    //<Ԥ���壬ʵ�����г�>
    Dictionary<T, Queue<T>> objectPool = new Dictionary<T, Queue<T>>();

    /// <summary>
    /// ����Ԥ���壬�ӳ�����ץһ��ʵ��������û�о�����һ����
    /// </summary>
    public T DequeueInstance(T prefab) {
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue != null && queue.Count > 0) {
                return queue.Dequeue();
            }
        }
        //û�о��½���
        T obj = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.SetRelatePrefab(prefab);
        return obj;
    }

    /// <summary>
    /// ��ʵ���Ż����Ԥ����ĳ����
    /// </summary>
    public void EnqueueInstance(T obj, T prefab) {
        if(obj == null || prefab == null) {
            return;
        }
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue == null) {
                queue = objectPool[prefab] = new Queue<T>();
            }
        }
        else {
            queue = new Queue<T>();
            objectPool.Add(prefab, queue);
        }
        queue.Enqueue(obj);
    }
}

public class PrefabPool_General<T> where T : Component {
    //<Ԥ���壬ʵ�����г�>
    Dictionary<T, Queue<T>> objectPool = new Dictionary<T, Queue<T>>();

    /// <summary>
    /// ����Ԥ���壬�ӳ�����ץһ��ʵ��������û�о�����һ����
    /// </summary>
    public T DequeueInstance(T prefab) {
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue != null && queue.Count > 0) {
                return queue.Dequeue();
            }
        }
        //û�о��½���
        T obj = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return obj;
    }

    /// <summary>
    /// ��ʵ���Ż����Ԥ����ĳ����
    /// </summary>
    public void EnqueueInstance(T obj, T prefab) {
        if (obj == null || prefab == null) {
            return;
        }
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue == null) {
                queue = objectPool[prefab] = new Queue<T>();
            }
        }
        else {
            queue = new Queue<T>();
            objectPool.Add(prefab, queue);
        }
        queue.Enqueue(obj);
    }
}