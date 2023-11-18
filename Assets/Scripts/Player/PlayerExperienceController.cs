using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperienceController : MonoBehaviour
{
    public float expLength;
    public int currentLevel;
    public float currentExp;

    private void OnEnable()
    {
        EventHandler.PlayerGetExp += AtPlayerGetExp;
        EventHandler.StartNewGameEvent += AtStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.PlayerGetExp -= AtPlayerGetExp;
        EventHandler.StartNewGameEvent += AtStartNewGameEvent;
    }

    private void AtPlayerGetExp(int experience)
    {
        currentExp += experience;
        if (currentExp >= expLength)
        {
            currentLevel++;
            EventHandler.CallUpdateExpBar(currentLevel, (currentExp - expLength) / expLength);
            EventHandler.CallPauseWithLevelUpEvent();
            currentExp -= expLength;
        }
        else
        {
            EventHandler.CallUpdateExpBar(currentLevel, currentExp / expLength);
        }
    }
    
    private void AtStartNewGameEvent()
    {
        currentLevel = 1;
        expLength = 50;
        currentExp = 0;
        EventHandler.CallUpdateExpBar(currentLevel, 0);
    }
}
