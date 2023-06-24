using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool<T> where T : RecycleObject<T> {
    //<预制体，实例队列池>
    Dictionary<T, Queue<T>> objectPool = new Dictionary<T, Queue<T>>();

    /// <summary>
    /// 根据预制体，从池子里抓一个实例出来，没有就生成一个。
    /// </summary>
    public T DequeueInstance(T prefab) {
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue != null && queue.Count > 0) {
                return queue.Dequeue();
            }
        }
        //没有就新建。
        T obj = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.SetRelatePrefab(prefab);
        return obj;
    }

    /// <summary>
    /// 把实例放回相关预制体的池子里。
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
    //<预制体，实例队列池>
    Dictionary<T, Queue<T>> objectPool = new Dictionary<T, Queue<T>>();

    /// <summary>
    /// 根据预制体，从池子里抓一个实例出来，没有就生成一个。
    /// </summary>
    public T DequeueInstance(T prefab) {
        if (objectPool.TryGetValue(prefab, out Queue<T> queue)) {
            if (queue != null && queue.Count > 0) {
                return queue.Dequeue();
            }
        }
        //没有就新建。
        T obj = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return obj;
    }

    /// <summary>
    /// 把实例放回相关预制体的池子里。
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