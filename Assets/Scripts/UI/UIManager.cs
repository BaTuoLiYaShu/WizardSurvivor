using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //TODO:死亡菜单再来一局游戏重新开一局
    
    private GameObject playerCanvas;
    private GameObject buffCanvas;
    private GameObject menuCanvas;
    public GameObject menuPrefab;
    
    public GameObject deadPanel;

    private void Awake()
    {
        playerCanvas = GameObject.FindWithTag("PlayerCanvas");
        buffCanvas = GameObject.FindWithTag("BuffCanvas");
        menuCanvas = GameObject.FindWithTag("MenuCanvas");
        Instantiate(menuPrefab, menuCanvas.transform);
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += AtAfterSceneLoadedEvent;
        EventHandler.EndGameEvent += AtEndGameEvent;
        EventHandler.PlayerDieEvent += AtPlayerDieEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= AtAfterSceneLoadedEvent;
        EventHandler.EndGameEvent -= AtEndGameEvent;
        EventHandler.PlayerDieEvent -= AtPlayerDieEvent;
    }

    private void AtAfterSceneLoadedEvent()
    {
        playerCanvas.SetActive(true);
        buffCanvas.SetActive(true);
        if (menuCanvas.transform.childCount > 0)
        {
            Destroy(menuCanvas.transform.GetChild(0).gameObject);
        }
    }
    
    private void AtEndGameEvent()
    {
        playerCanvas.SetActive(false);
        buffCanvas.SetActive(false);
    }
    
    private void AtPlayerDieEvent()
    {
        deadPanel.SetActive(true);
    }

    public void ReplayGame()
    {
        deadPanel.SetActive(false);
        StartCoroutine(RestartPlayer());
    }

    private IEnumerator RestartPlayer()
    {
        yield return TransitionManager.instance.Fade(1);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        EventHandler.CallStartNewGameEvent();
        yield return TransitionManager.instance.Fade(0);
    }
    
    public void ReturnMenuCanvas()
    {
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        deadPanel.SetActive(false);
        EventHandler.CallEndGameEvent();
        yield return new WaitForSeconds(1f);
        Instantiate(menuPrefab, menuCanvas.transform);
    }
    
}
