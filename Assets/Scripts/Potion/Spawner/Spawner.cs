using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : SetupBehaviour
{
    [SerializeField] protected Transform holder;
    public Transform Holder => holder;
    [SerializeField] protected List<Transform> _listPrefabs;
    [SerializeField] protected List<Transform> _poolObjects;

    protected override void LoadComponents()
    {
        GetPrefabs();
        GetHolder();
    }

    protected virtual void GetPrefabs()
    {
        if (_listPrefabs.Count > 0) return;
        Transform prefabs = transform.Find("Prefabs");
        foreach (Transform child in prefabs)
        {
            _listPrefabs.Add(child);
        }
        UnactivePrefabs();
        Debug.Log("Reset " + this.GetType().Name + " : " + nameof(_listPrefabs));
    }
    protected virtual void GetHolder()
    {
        if (holder != null) return;
        holder = transform.Find("Holder");
        Debug.Log("Reset " + this.GetType().Name + " : " + nameof(holder));
    }

    protected virtual void UnactivePrefabs()
    {
        Transform prefabs = transform.Find("Prefabs");
        foreach (Transform child in prefabs)
        {
            child.gameObject.SetActive(false);
        }
    }
    public virtual Transform Spawn(string prefabName, Vector3 position, Quaternion rotation)
    {
        Transform prefab = GetPrefabByName(prefabName);
        if (prefab == null) return null;
        Transform newPrefab = GetPrefabFromPool(prefab);
        newPrefab.SetParent(holder);
        newPrefab.SetPositionAndRotation(position, rotation);
        newPrefab.gameObject.SetActive(true);
        return newPrefab;
    }
    public virtual Transform GetPrefabBeforeSpawn(string prefabName)
    {
        Transform prefab = GetPrefabByName(prefabName);
        if (prefab == null) return null;
        Transform newPrefab = GetPrefabFromPool(prefab);
        return newPrefab;
    }
    public virtual Transform SpawnAfterGetPrefab(Transform prefab, Vector3 position, Quaternion rotation)
    {
        prefab.SetParent(holder);
        prefab.SetPositionAndRotation(position, rotation);
        prefab.gameObject.SetActive(true);
        return prefab;
    }

    protected virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in _listPrefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

    protected virtual Transform GetPrefabFromPool(Transform prefab)
    {
        foreach (Transform objectInPool in _poolObjects)
        {
            if (objectInPool.name == prefab.name)
            {
                _poolObjects.Remove(objectInPool);
                return objectInPool;
            }
        }
        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    public virtual void Despawn(Transform prefab)
    {
        prefab.gameObject.SetActive(false);
        _poolObjects.Add(prefab);
    }
    public virtual float GetRandomOffsetPosition(float min, float max)
    {
        float offset = Random.Range(min, max);
        return offset;
    }
    protected virtual void DespawnAllPrefabs()
    {
        foreach (Transform objectInHolder in holder)
        {
            if (objectInHolder.gameObject.activeSelf)
                Despawn(objectInHolder);
        }
    }
}