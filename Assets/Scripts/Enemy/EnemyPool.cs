using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;

    [SerializeField]
    private int numEnemiesPerPool;
    [SerializeField]
    private GameObject PufferPrefab;
    [SerializeField]
    private GameObject EelPrefab;
    [SerializeField]
    private GameObject MantaPrefab;

    private Dictionary<string, Pool> enemyPoolMap;

	void Start ()
    {
        if (instance == null)
        {
            enemyPoolMap = new Dictionary<string, Pool>();
            enemyPoolMap[Tags.Enemy.PufferFish] = new Pool(transform, PufferPrefab, numEnemiesPerPool);
            enemyPoolMap[Tags.Enemy.Eel] = new Pool(transform, EelPrefab, numEnemiesPerPool);
            enemyPoolMap[Tags.Enemy.MantaRay] = new Pool(transform, MantaPrefab, numEnemiesPerPool);
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

        int r = Random.Range(0,3);
        if (r == 0)
        {
            enemyPoolMap[Tags.Enemy.PufferFish].Spawn(spawnPosition);
        }
        else if (r == 1)
        {
            enemyPoolMap[Tags.Enemy.Eel].Spawn(spawnPosition);
        }
        else if (r == 2)
        {
            enemyPoolMap[Tags.Enemy.MantaRay].Spawn(spawnPosition);
        }
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        enemyPoolMap[enemy.tag].Free(enemy);
    }

    public struct Pool
    {
        public HashSet<GameObject> free;
        public HashSet<GameObject> inUse;
        private int totalEnemiesInPool;

        public Pool(Transform parent, GameObject enemyPrefab, int enemiesPerPool)
        {
            free = new HashSet<GameObject>();
            inUse = new HashSet<GameObject>();
            totalEnemiesInPool = enemiesPerPool;

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

            UnityEngine.Assertions.Assert.AreEqual(free.Count + inUse.Count, totalEnemiesInPool);
        }

        public void Free(GameObject pufferToFree)
        {
            inUse.Remove(pufferToFree);
            free.Add(pufferToFree);
            pufferToFree.SetActive(false);

            UnityEngine.Assertions.Assert.AreEqual(free.Count + inUse.Count, totalEnemiesInPool);
        }
    }
}
