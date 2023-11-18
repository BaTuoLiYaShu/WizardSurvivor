using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    private CanvasGroup fadeCanvasGroup;
    
    private void Awake()
    {
        instance = GetComponent<TransitionManager>();
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        EventHandler.CallEndGameEvent();
        EventHandler.CallPlaySoundEvent(SceneType.菜单);
    }

    private void Start()
    {
        fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
    }

    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += AtStartNewGameEvent;
        EventHandler.EndGameEvent += AtEndGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= AtStartNewGameEvent;
        EventHandler.EndGameEvent -= AtEndGameEvent;
    }

    private void AtStartNewGameEvent()
    {
        StartCoroutine(LoadSceneAndSetActive("Grass"));
    }
    
    private void AtEndGameEvent()
    {
        StartCoroutine(UnloadScene());
    }
    
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return Fade(1);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        var sceneType = GetSceneTypeOfScene(sceneName); 
        
        EventHandler.CallPlaySoundEvent(sceneType); //播放对应场景声音
        EventHandler.CallAfterSceneLoadedEvent();
        
        var newScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(newScene); //激活场景
        yield return Fade(0);
    }

    private SceneType GetSceneTypeOfScene(string sceneName)
    {
        return sceneName switch
        {
            "Grass" => SceneType.关卡,
            "Menu" => SceneType.菜单,
            _ => SceneType.菜单
        };
    }
    
    private IEnumerator UnloadScene()
    {
        yield return Fade(1f);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return Fade(0);
    }
    
    /// <summary>
    /// 淡入淡出场景
    /// </summary>
    /// <param name="targetAlpha">1是黑, 0是透明</param>
    /// <returns></returns>
    public IEnumerator Fade(float targetAlpha)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        var speed = Math.Abs(fadeCanvasGroup.alpha - targetAlpha) / 1.0f;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
    }
}
