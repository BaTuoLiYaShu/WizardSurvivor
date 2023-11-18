using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BossBar : MonoBehaviour
{
    public GameObject bossProfile;
    public TextMeshProUGUI textMeshPro;

    private void OnEnable()
    {
        EventHandler.ShowBossNameEvent += AtShowBossNameEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowBossNameEvent -= AtShowBossNameEvent;
    }

    private void AtShowBossNameEvent(string name)
    {
        StartCoroutine(ShowBossName(name));
    }

    private IEnumerator ShowBossName(string name)
    {
        bossProfile.SetActive(true);
        textMeshPro.text = "Boss" + name + "登场";
        yield return new WaitForSeconds(3);
        bossProfile.SetActive(false);
    }
}
