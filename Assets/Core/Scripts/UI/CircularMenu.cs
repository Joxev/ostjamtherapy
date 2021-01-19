﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CircularMenu : MonoBehaviour
{
    public Transform textPointHolder;

    public int GameSceneIndex = 2;
    public int CreditsSceneIndex = 3;
    public int SettingsSceneIndex = 4;

    float value = 1;

    public bool isHover = false;

    public bool hasClicked = false;

    public float targetRotation;

    public GameObject carrotUi;

    bool animDone = false;

    private void Start()
    {
        animDone = false;
        carrotUi.SetActive(false);
    }

    private void Update()
    {
        if (hasClicked)
        {
            if (Mathf.Abs(textPointHolder.transform.localEulerAngles.z - targetRotation) > 0.5f)
            {
                if(textPointHolder.transform.localEulerAngles.z < targetRotation)
                {
                    value += 0.3f;
                }
                else
                {
                    value -= 0.3f;
                }
            }
            else
            {
                animDone = true;
                carrotUi.SetActive(true);
            }
        }
        else if(!isHover)
        {
            if (value > 360)
            {
                value = 0;
            }
            else
            {
                value += 0.03f;
            }
        }
        else
        {
            value += Mathf.Sin(Time.time * 2) * 0.003f;
        }
        textPointHolder.transform.rotation = Quaternion.Euler(textPointHolder.transform.rotation.x, textPointHolder.transform.rotation.y, value);
    }

    public void onClick(float targetRot)
    {
        if(!hasClicked)
        {
            targetRotation = targetRot;
            hasClicked = true;
        }
    }

    public void StartCredits()
    {
        //DontDestroyOnLoad(SoundManager.instance);
        SceneManager.LoadScene(CreditsSceneIndex);
    }
    public void StartSettings()
    {
        SceneManager.LoadScene(SettingsSceneIndex);
    }
    public void StartGame()
    {
        StartCoroutine(startGameWait());
    }
    public void BackButton()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator startGameWait()
    {
        if (animDone)
        {
            SoundManager.instance.GameplaySound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            SceneManager.LoadScene(GameSceneIndex);
        }
        yield return null;
        StartCoroutine(startGameWait());
    }
}

