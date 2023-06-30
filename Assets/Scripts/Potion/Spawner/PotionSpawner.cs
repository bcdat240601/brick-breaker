using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : Spawner
{
    protected static PotionSpawner instance;
    public static PotionSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogError("there's 2 PotionSpawner in the scene");
        }
        instance = this;
    }
    public virtual void SpawnRandomObject(Vector3 potion, Quaternion rotation)
    {
        int random1 = Random.Range(1, 11);
        int random2;
        int randomIndexItem = Random.Range(1, 11);
        if (random1 != randomIndexItem) return;
        random2 = Random.Range(1, 5);
        PotionType potionType = (PotionType)random2;        
        Spawn(potionType.ToString(), potion, rotation);

    }
}
