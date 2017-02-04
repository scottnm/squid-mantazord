using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [SerializeField]
    int numEnemiesPerPool;
    [SerializeField]
    GameObject PufferPrefab;
    [SerializeField]
    GameObject EelPrefab;
    [SerializeField]
    GameObject MantaPrefab;

    public struct Pool
    {
        public HashSet<GameObject> free;
        public HashSet<GameObject> inUse;

        public Pool(Transform parent, GameObject enemyPrefab, int enemiesPerPool)
        {
            free = new HashSet<GameObject>();
            inUse = new HashSet<GameObject>();

            for (int i = 0; i < enemiesPerPool; ++i)
            {
                var newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, parent);
                newEnemy.SetActive(false);
                free.Add(newEnemy);
            }
        }

        public void Spawn(Vector2 spawnPosition)
        {
            var itr = free.GetEnumerator();
            itr.MoveNext();
            var nextPuffer = itr.Current;

            free.Remove(nextPuffer);
            inUse.Add(nextPuffer);
            nextPuffer.SetActive(true);
            nextPuffer.transform.position = spawnPosition;
        }

        public void Free(GameObject pufferToFree)
        {
            inUse.Remove(pufferToFree);
            free.Add(pufferToFree);
            pufferToFree.SetActive(false);
        }
    }

    Pool PufferPool;
    Pool EelPool;
    Pool MantaPool;

	void Start ()
    {
        if (instance == null)
        {
            PufferPool = new Pool(transform, PufferPrefab, numEnemiesPerPool);
            EelPool = new Pool(transform, EelPrefab, numEnemiesPerPool);
            MantaPool = new Pool(transform, MantaPrefab, numEnemiesPerPool);
            instance = this;
            Events.OnEnemyDeath += OnEnemyDeath;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
            Events.OnEnemyDeath -= OnEnemyDeath;
        }
    }

    public void Spawn(EnemyType et)
    {
        Events.EnemySpawned(et);
        var spawnPosition = ArenaGenerator.GetGridInstance().GetRandomSpawn();

        int r = (int)Mathf.Floor(Random.Range(0,3));
        if (r == 0)
        {
            PufferPool.Spawn(spawnPosition);
        }
        else if (r == 1)
        {
            EelPool.Spawn(spawnPosition);
        }
        else if (r == 2)
        {
            MantaPool.Spawn(spawnPosition);
        }
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        PufferPool.Free(enemy);
    }
}
