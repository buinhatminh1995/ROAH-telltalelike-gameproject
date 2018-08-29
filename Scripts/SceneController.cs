using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private bool loadingScene;
    private GameObject overlay;

    private static SceneController _instance;
    public static SceneController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        var ink = FindObjectOfType<InkScript>();
        ink.GameEventFired += OnGameEventFired;

        overlay = GameObject.Find("Overlay");
    }

    private void Update()
    {

        // [ key load previous scene
        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
            var num = SceneManager.GetActiveScene().buildIndex - 1;
            switch (num)
            {
                case 1:
                    OnGameEventFired("load_sc2_airplane");
                    break;

                case 2:
                    OnGameEventFired("load_sc3a_eastmansion");
                    break;

                case 3:
                    OnGameEventFired("load_sc3b_eastlab");
                    break;

                case 4:
                    OnGameEventFired("load_sc4_coffeeshop");
                    break;

                case 5:
                    OnGameEventFired("load_sc5_alley");
                    break;

                case 6:
                    OnGameEventFired("load_sc6_apartment");
                    break;

                case 7:
                    OnGameEventFired("load_sc7_ritual");
                    break;

                case 8:
                    OnGameEventFired("load_endgame");
                    break;
            }
        }

        // ] key load next scene
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            var num = SceneManager.GetActiveScene().buildIndex + 1;
            switch (num)
            {
                case 1:
                    OnGameEventFired("load_sc2_airplane");
                    break;

                case 2:
                    OnGameEventFired("load_sc3a_eastmansion");
                    break;

                case 3:
                    OnGameEventFired("load_sc3b_eastlab");
                    break;

                case 4:
                    OnGameEventFired("load_sc4_coffeeshop");
                    break;

                case 5:
                    OnGameEventFired("load_sc5_alley");
                    break;

                case 6:
                    OnGameEventFired("load_sc6_apartment");
                    break;

                case 7:
                    OnGameEventFired("load_sc7_ritual");
                    break;

                case 8:
                    OnGameEventFired("load_endgame");
                    break;
            }
        }

    }

    public void OnGameEventFired(string gameEvent)
    {
        Debug.Log(gameEvent);
        switch (gameEvent)
        {
            case "load_sc2_airplane":
                LoadScene(1, 6f);
                break;

            case "load_sc3a_eastmansion":
                LoadScene(2, 4f);
                break;

            case "load_sc3b_eastlab":
                LoadScene(3);
                break;

            case "load_sc4_coffeeshop":
                LoadScene(4);
                break;

            case "load_sc5_alley":
                LoadScene(5);
                break;

            case "load_sc6_apartment":
                LoadScene(6);
                break;

            case "load_sc7_ritual":
                LoadScene(7);
                break;

            case "load_endgame":
                LoadScene(8);
                break;
        } 
    }

    public void LoadScene(int sceneIndex = -1, float fadeOutDelay = 2f, float fadeInDelay = 1f)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex, fadeOutDelay, fadeInDelay));
    }

    IEnumerator LoadSceneAsync(int sceneIndex, float fadeOutDelay, float fadeInDelay)
    {
        var overlaycg = overlay.GetComponent<CanvasGroup>();
        var overlayBlock = overlay.GetComponent<Image>();

        if (loadingScene)
            yield return null;
        loadingScene = true;
        overlayBlock.raycastTarget = true;
        yield return StartCoroutine(fadeOut(overlaycg, 2f));
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //print("scene unload");
        yield return new WaitForSecondsRealtime(fadeOutDelay);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        yield return new WaitForSecondsRealtime(1f + fadeInDelay);
        yield return StartCoroutine(fadeIn(overlaycg, 2f));
        overlayBlock.raycastTarget = false;
        loadingScene = false;

    }

    public IEnumerator fadeOut(CanvasGroup cg, float seconds)
    {
        var fadeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        var canvas = cg;
        //if (canvas.alpha != 0f) yield break;
        for (float start = 0f, end = 1f, t = 0f; t <= 1f; t += Time.unscaledDeltaTime / seconds)
        {
            if (canvas)
                canvas.alpha = Mathf.Lerp(start, end, fadeCurve.Evaluate(t));
            yield return null;
        }
        canvas.alpha = 1f;
    }

    public IEnumerator fadeIn(CanvasGroup cg, float seconds)
    {
        var fadeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        var canvas = cg;
        //if (canvas.alpha != 1f) yield break;
        for (float start = 1f, end = 0f, t = 0f; t <= 1f; t += Time.unscaledDeltaTime / seconds)
        {
            if (canvas)
                canvas.alpha = Mathf.Lerp(start, end, fadeCurve.Evaluate(t));
            yield return null;
        }
        canvas.alpha = 0f;
    }
}