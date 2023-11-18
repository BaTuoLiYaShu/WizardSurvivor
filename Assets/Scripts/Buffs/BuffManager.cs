using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private Player player;
    private Gun gun;
    public Bullet bullet;
    
    private void OnEnable()
    {
        EventHandler.AfterSelectBuffEvent += AtAfterSelectBuffEvent;
        EventHandler.AfterSceneLoadedEvent += AtAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSelectBuffEvent -= AtAfterSelectBuffEvent;
        EventHandler.AfterSceneLoadedEvent -= AtAfterSceneLoadedEvent;
    }

    private void AtAfterSelectBuffEvent(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.伤害:
                bullet.damage += 2;
                break;
            case BuffType.射速:
                gun.fireSpeed -= 0.2f;
                break;
            case BuffType.速度:
                player.speed += 1;
                break;
            case BuffType.吸血:
                bullet.vampirePercentage += 3;
                break;
            case BuffType.血量:
                //给Player增加最大血量，同时增加血条
                EventHandler.CallAddMaxHeartEvent();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buffType), buffType, null);
        }
    }
    
    private void AtAfterSceneLoadedEvent()
    {
        player = FindObjectOfType<Player>();
        gun = FindObjectOfType<Gun>();
    }
}
