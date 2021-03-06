﻿using UnityEngine;

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

    public delegate void PlayerDamageListener();
    public static event PlayerDamageListener OnPlayerDamage;
    public static void PlayerDamaged()
    {
        if (OnPlayerDamage != null)
        {
            OnPlayerDamage();
        }
    }

    public delegate void PlayerDeathListener();
    public static event PlayerDeathListener OnPlayerDeath;
    public static void PlayerDies()
    {
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    public delegate void PlayerHealthChangedListener(int health);
    public static event PlayerHealthChangedListener OnPlayerHealthChange;
    public static void PlayerHealthChanges(int health)
    {
        if (OnPlayerHealthChange != null)
        {
            OnPlayerHealthChange(health);
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

    public delegate void ScreenShakeBeginListener();
    public static event ScreenShakeBeginListener OnScreenShakeBegin;
    public static void ScreenShakeBegin()
    {
        if (OnScreenShakeBegin != null)
        {
            OnScreenShakeBegin();
        }
    }

    public delegate void ScreenShakeEndListener();
    public static event ScreenShakeEndListener OnScreenShakeEnd;
    public static void ScreenShakeEnd()
    {
        if (OnScreenShakeEnd != null)
        {
            OnScreenShakeEnd();
        }
    }

    public delegate void StartGameListener();
    public static event StartGameListener OnStartGame;
    public static void StartGame()
    {
        if (OnStartGame != null)
        {
            OnStartGame();
        }
    }

    public delegate void GameOverListener();
    public static event GameOverListener OnGameOver;
    public static void GameOver()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }
}
