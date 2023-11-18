using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject heart;

    public Sprite fullHeartSprite;
    public Sprite halfEmptyHeartSprite;
    public Sprite emptyHeartSprite;
    public Transform healthBar;
    
    public List<Image> hearts;

    private void Start()
    {
        hearts = new List<Image>();
    }

    private void OnEnable()
    {
        EventHandler.UpdateHealthBar += AtUpdateHealthBar;
        EventHandler.AddMaxHeartEvent += AtAddMaxHeartEvent;
        EventHandler.StartNewGameEvent += AtStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateHealthBar -= AtUpdateHealthBar;
        EventHandler.AddMaxHeartEvent -= AtAddMaxHeartEvent;
        EventHandler.StartNewGameEvent -= AtStartNewGameEvent;
    }

    private void AtStartNewGameEvent()
    {
        InitHeartHealth();
    }

    #region 事件

    private void AtUpdateHealthBar(float health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i == (int)Mathf.Ceil(health / 2) - 1)
            {
                if (health % 2 != 0)
                {
                    hearts[i].sprite = halfEmptyHeartSprite;
                }
                else
                {
                    hearts[i].sprite = fullHeartSprite;
                }
            }
            else if (i > (int)Mathf.Ceil(health / 2) - 1)
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
    
    private void AtAddMaxHeartEvent()
    {
        var obj = Instantiate(heart, healthBar);
        obj.GetComponent<Image>().sprite = emptyHeartSprite;
        hearts.Add(obj.GetComponent<Image>());
    }

    #endregion
    
    private void InitHeartHealth()
    {
        foreach (var h in hearts)
        {
            Destroy(h.gameObject);
        }
        
        hearts.Clear();
        
        for (int i = 0; i < 3; i++)
        {
            var h = Instantiate(heart, healthBar);
            hearts.Add(h.GetComponent<Image>());
        }
    }
}
