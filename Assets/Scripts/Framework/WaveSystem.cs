using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class ActSpecification
    {
        public WaveSpecification[] waveSpecs;
    }

    [System.Serializable]
    public class WaveSpecification
    {
        public EnemySetSpecification enemySpec;
        public int additionalSpawns;
        public AnimationCurve spawnCurve;
        public float spawnTime;
    }

    [System.Serializable]
    public class EnemySetSpecification
    {
        public int numberPuffers;
        private int pufferCount;

        public EnemyType PopRandomEnemy()
        {
            ++pufferCount;
            return EnemyType.Pufferfish;
        }

        public int TotalEnemies()
        {
            return numberPuffers;
        }

        public int EnemiesLeft()
        {
            return numberPuffers - pufferCount;
        }

        public int EnemiesSpawned()
        {
            return pufferCount;
        }
    }

    [SerializeField]
    ActSpecification act1;
    /*
    [SerializeField]
    ActSpecification act2;
    [SerializeField]
    ActSpecification act3;
    */

    ActSpecification[] acts;
    int numEnemiesKilled;

	// Use this for initialization
	void Start ()
    {
        acts = new ActSpecification[1];//3];
        acts[0] = act1;
        //acts[1] = act2;
        //acts[2] = act3;
        Events.OnEnemyDeath += OnEnemyDeath;
        StartCoroutine(BeginInitialWave());
	}

    private void OnDestroy()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        ++numEnemiesKilled;
    }

    IEnumerator BeginInitialWave()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(LaunchWave(0, 0));
    }

    IEnumerator LaunchWave(int actnumber, int wavenumber)
    {
        Debug.Log("Launch Wave");
        ActSpecification currentAct = acts[actnumber];
        if (wavenumber >= currentAct.waveSpecs.Length)
        {
            Events.EndAct();
            yield break;
        }

        numEnemiesKilled = 0;
        Events.StartWave();
        float time = 0;
        WaveSpecification waveSpec = currentAct.waveSpecs[wavenumber];
        ArenaGenerator.GetGridInstance().AddSpawnPoints(waveSpec.additionalSpawns);
        
        while (waveSpec.enemySpec.EnemiesLeft() > 0)
        {
            float percentageEnemiesSpawned = (1.0f * waveSpec.enemySpec.EnemiesSpawned()) / waveSpec.enemySpec.TotalEnemies();
            var timeToNextEnemy = waveSpec.spawnTime * waveSpec.spawnCurve.Evaluate(percentageEnemiesSpawned) - time;
            if (timeToNextEnemy > 0)
            {
                yield return new WaitForSeconds(timeToNextEnemy);
                time += timeToNextEnemy;
            }
            EnemyPool.instance.Spawn(waveSpec.enemySpec.PopRandomEnemy());
        }

        Debug.Log("All Enemies Spawned");
        while (numEnemiesKilled < waveSpec.enemySpec.TotalEnemies())
        {
            yield return null;
        }
        Debug.Log("All Enemies killed");

        yield return new WaitForSeconds(1f);

        StartCoroutine(LaunchWave(actnumber, wavenumber + 1));
    }
}
