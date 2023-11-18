using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlotUI : MonoBehaviour
{
    public Image buffImage;
    public TextMeshProUGUI description;
    public Button selectButton;

    private BuffType buffType;
    private int index;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
    }

    private void Start()
    {
        selectButton.onClick.AddListener(SelectBuff);
    }

    public void UpdateBuffSlot(Buff buff)
    {
        buffImage.sprite = buff.icon;
        description.text = buff.description;
        buffType = buff.buffType;
    }

    private void SelectBuff()
    {
        EventHandler.CallAfterSelectBuffEvent(buffType);
        EventHandler.CallPauseWithLevelUpEvent();
    }
}
