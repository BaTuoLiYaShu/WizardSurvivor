using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventHandler
{
    //伤害玩家事件
    public static event Action<int> DamagePlayerEvent;
    public static void CallDamagePlayerEvent(int damage)
    {
        DamagePlayerEvent?.Invoke(damage);
    }

    public static event Action PlayerDieEvent;
    public static void CallPlayerDieEvent()
    {
        PlayerDieEvent?.Invoke();
    }
    
    public static event Action RestorePlayerHealthEvent;
    public static void CallRestorePlayerHealthEvent()
    {
        RestorePlayerHealthEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPositionEvent;
    public static void CallMoveToPositionEvent(Vector3 targetPosition)
    {
        MoveToPositionEvent?.Invoke(targetPosition);
    }

    public static event Action CameraShakeEvent;
    public static void CallCameraShakeEvent()
    {
        CameraShakeEvent?.Invoke();
    }

    public static event Action StartNewGameEvent;
    public static void CallStartNewGameEvent()
    {
        StartNewGameEvent?.Invoke();
    }
    
    public static event Action EndGameEvent;
    public static void CallEndGameEvent()
    {
        EndGameEvent?.Invoke();
    }

    public static event Action<int> PlayerGetExp;
    public static void CallPlayerGetExp(int experience)
    {
        PlayerGetExp?.Invoke(experience);
    }

    public static event Action<Vector3> EnemyDropExp;
    public static void CallEnemyDropExp(Vector3 pos)
    {
        EnemyDropExp?.Invoke(pos);
    }

    public static event Action<int, float> UpdateExpBar;
    public static void CallUpdateExpBar(int level, float expPercentage)
    {
        UpdateExpBar?.Invoke(level, expPercentage);
    }

    public static event Action<float> UpdateHealthBar;
    public static void CallUpdateHealthBar(float health)
    {
        UpdateHealthBar?.Invoke(health);
    }

    public static event Action AddMaxHeartEvent;
    public static void CallAddMaxHeartEvent()
    {
        AddMaxHeartEvent?.Invoke();
    }
    
    public static event Action PauseWithLevelUpEvent;
    public static void CallPauseWithLevelUpEvent()
    {
        PauseWithLevelUpEvent?.Invoke();
    }

    public static event Action<BuffType> AfterSelectBuffEvent;
    public static void CallAfterSelectBuffEvent(BuffType buffType)
    {
        AfterSelectBuffEvent?.Invoke(buffType);
    }

    public static event Action<string> ShowBossNameEvent;
    public static void CallShowBossNameEvent(string name)
    {
        ShowBossNameEvent?.Invoke(name);
    }

    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<SceneType> PlaySoundEvent;
    public static void CallPlaySoundEvent(SceneType sceneType)
    {
        PlaySoundEvent?.Invoke(sceneType);
    }
}
