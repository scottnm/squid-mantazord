using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct AudioResources
    {
        public AudioClip mainGameMusic;
        public AudioClip[] playerAttack;
        public AudioClip playerDeath;
        public AudioClip playerHurt;
        public AudioClip enemyDeath;
        public AudioClip screenShake;
    }

    [SerializeField]
    private AudioResources soundbank;

    private AudioSource mMusicPlayer;
    private AudioSource mSFXPlayer;

	void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        mMusicPlayer = audioSources[0];
        mSFXPlayer = audioSources[1];

        mMusicPlayer.clip = soundbank.mainGameMusic;
        mMusicPlayer.volume = .7f;
        mMusicPlayer.loop = true;
        mMusicPlayer.Play();

	    Events.OnEnemyDeath += OnEnemyDeath;
        Events.OnPlayerDamage += OnPlayerDamage;
	    Events.OnPlayerDeath += OnPlayerDeath;
        Events.OnScreenShakeBegin += OnScreenShakeBegin;
        Events.OnScreenShakeEnd += OnScreenShakeEnd;
	}
	
	void OnDestroy()
    {
	    Events.OnEnemyDeath -= OnEnemyDeath;
        Events.OnPlayerDamage -= OnPlayerDamage;
	    Events.OnPlayerDeath -= OnPlayerDeath;
        Events.OnScreenShakeBegin -= OnScreenShakeBegin;
        Events.OnScreenShakeEnd -= OnScreenShakeEnd;
	}

    private void OnEnemyDeath(GameObject enemy)
    {
        mSFXPlayer.PlayOneShot(soundbank.enemyDeath);
    }

    private void OnPlayerAttack()
    {
        mSFXPlayer.PlayOneShot(
            Utility.RandomElement<AudioClip>(soundbank.playerAttack));
    }

    private void OnPlayerDamage()
    {
        mSFXPlayer.PlayOneShot(soundbank.playerHurt);
    }

    private void OnPlayerDeath()
    {
        mSFXPlayer.PlayOneShot(soundbank.playerDeath);
    }

    private void OnScreenShakeBegin()
    {
        mSFXPlayer.clip = soundbank.screenShake;
        mSFXPlayer.Play();
    }

    private void OnScreenShakeEnd()
    {
        mSFXPlayer.Stop();
    }
}
