using UnityEngine;

public class Events
{
    public delegate void WaveStartListener();
    public static event WaveStartListener OnWaveStart;
    public static void StartWave()
    {
        if (OnWaveStart != null)
        {
            OnWaveStart();
        }
    }

    public delegate void EnemyDeathListener(GameObject enemy);
    public static event EnemyDeathListener OnEnemyDeath;
    public static void EnemyDies(GameObject enemy)
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(enemy);
        }
    }

    public delegate void EndActListener();
    public static event EndActListener OnEndAct;
    public static void EndAct()
    {
        if (OnEndAct != null)
        {
            OnEndAct();
        }
    }

    public delegate void EnemySpawnedListener(EnemyType enemyType);
    public static event EnemySpawnedListener OnEnemySpawn;
    public static void EnemySpawned(EnemyType enemyType)
    {
        if (OnEnemySpawn != null)
        {
            OnEnemySpawn(enemyType);
        }
    }
}
