using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource gameMusicSource;
    
    public AudioClip menuMusicClip;
    public AudioClip playMusicClip;

    private void OnEnable()
    {
        EventHandler.PlaySoundEvent += AtPlaySoundEvent;
    }

    private void OnDisable()
    {
        EventHandler.PlaySoundEvent -= AtPlaySoundEvent;
    }

    private void AtPlaySoundEvent(SceneType sceneType)
    {
        gameMusicSource.clip = sceneType == SceneType.关卡 ? playMusicClip : menuMusicClip;

        gameMusicSource.Play();
    }
}
