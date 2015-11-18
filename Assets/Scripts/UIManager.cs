﻿using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public bool allowSceneActivation = false;

    public GameObject FadePlane;
    public GameObject MenuLayout;
    public GameObject GameLayout;
    public GameObject LoadingScreen;
    public float fadeTime;
    
    private Animator anim;
    private WaitForSeconds halfOfTime;
    private WaitForSeconds time;
    private WaitForEndOfFrame wfeof;

    void Start()
    {
        halfOfTime = new WaitForSeconds(fadeTime / 2);
        time = new WaitForSeconds(fadeTime);
        wfeof = new WaitForEndOfFrame();
        anim = FadePlane.GetComponent<Animator>();
        anim.SetFloat("speedMultiplier", 1 / fadeTime);
    } 

    public void Restart()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel));
    }

    public void StartGame()
    {
        StartCoroutine(WaitForLoad(Application.loadedLevel + 1));
    }

    public void ExitGame()
    {
        StartCoroutine(WaitForExit());
    }

    IEnumerator WaitForExit()
    {
        anim.SetBool("Fade", true);
        yield return halfOfTime;
        Application.Quit();
    }

    IEnumerator WaitForLoad(int level)
    {
        anim.SetBool("Fade", true);
        yield return halfOfTime;
        if (!LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(true); }
        anim.SetBool("Fade", false);
        yield return time;
        //yield return halfOfTime;

        AsyncOperation async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;
        while (async.progress < 0.9f) { yield return wfeof; }

        anim.SetBool("Fade", true);
        yield return halfOfTime;
        async.allowSceneActivation = true;
        anim.SetBool("Fade", false);
    }

    void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                if (GameLayout.activeInHierarchy) { GameLayout.SetActive(false); }
                if (LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(false); }
                if (!MenuLayout.activeInHierarchy) { MenuLayout.SetActive(true); }
                break;
            default:
                if (!GameLayout.activeInHierarchy) { GameLayout.SetActive(true); }
                if (MenuLayout.activeInHierarchy) { MenuLayout.SetActive(false); }
                if (LoadingScreen.activeInHierarchy) { LoadingScreen.SetActive(false); }
                break;
        }
    }
}
