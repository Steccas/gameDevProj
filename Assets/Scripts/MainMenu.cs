﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    public void PlayGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(Load(operation));
        audioManager.Stop("MainTheme");
        audioManager.Play("Gameplay");
    }
}
