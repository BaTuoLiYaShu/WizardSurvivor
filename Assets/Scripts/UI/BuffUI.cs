using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuffUI : MonoBehaviour
{
    public BuffDataList_SO buffData;
    public GameObject buffUI;
    public List<BuffSlotUI> buffSlotList;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
    }

    private void OnEnable()
    {
        EventHandler.PauseWithLevelUpEvent += AtPauseWithLevelUpEvent;
    }

    private void OnDisable()
    {
        EventHandler.PauseWithLevelUpEvent -= AtPauseWithLevelUpEvent;
    }

    private void AtPauseWithLevelUpEvent()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            SetBuff();
            buffUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            buffUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void SetBuff()
    {
        List<Buff> tempBuffList = new List<Buff>();
        while (true)
        {
            var tempBuff = buffData.buffDataList[Random.Range(0, buffData.buffDataList.Count)];
            if (!tempBuffList.Contains(tempBuff))
            {
                tempBuffList.Add(tempBuff);
            }

            if (tempBuffList.Count == 3)
            {
                break;
            }
        }

        for (int i = 0; i < tempBuffList.Count; i++)
        {
            buffSlotList[i].UpdateBuffSlot(tempBuffList[i]);
        }
    }
}
