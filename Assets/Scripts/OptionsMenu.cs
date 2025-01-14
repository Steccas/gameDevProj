﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider masterSFXSlider;
    public Toggle fsToggle;
    public Dropdown resDropdown;

    private PrefStorage<int> storageInt;
    private PrefStorage<float> storageFloat;

    Resolution[] resolutions;

    public void Awake()
    {
        storageInt = gameObject.AddComponent<PrefStorage<int>>();
        storageFloat = gameObject.AddComponent<PrefStorage<float>>();
        LoadPref("MasterVolume");
        LoadPref("MusicVolume");
        LoadPref("SFXMasterVolume");
        LoadPref("Fullscreen");
    }

    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resDropdown.ClearOptions();
        List<string> resOptions = new List<string>();

        int cResI = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resOptions.Add(option);

            Debug.Log(Screen.width);
            Debug.Log(Screen.height);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                cResI = i;
                Debug.Log("CurrentResFound");
            }
        }

        resDropdown.AddOptions(resOptions);
        resDropdown.value = cResI;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex) 
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMaster(float volume)
    {
        SetVolume("MasterVolume", volume);
        //SetPref("MasterVolume", volume);
        storageFloat.SetPref("MasterVolume", volume);
    }

    public void SetMusic(float volume)
    {
        SetVolume("MusicVolume", volume);
        storageFloat.SetPref("MusicVolume", volume);
    }

    public void SetSFX(float volume)
    {
        SetVolume("SFXMasterVolume", volume);
        storageFloat.SetPref("SFXMasterVolume", volume);
    }
    private void SetVolume(string value, float volume)
    {
        audioMixer.SetFloat(value, volume);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        int pref;
        if (isFullScreen) pref = 1;
        else pref = 0;
        Screen.fullScreen = isFullScreen;
        storageInt.SetPref("Fullscreen", pref);
        /*PlayerPrefs.SetInt("Fullscreen", pref);
        PlayerPrefs.Save();*/
    }
    void SetPref(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    void LoadPref(string key)
    {
        if (PlayerPrefs.HasKey(key) && key == "Fullscreen")
        {
            int value = PlayerPrefs.GetInt(key);
            if (value == 0) 
            { 
                Screen.fullScreen = false;
                fsToggle.SetIsOnWithoutNotify(false);
            }
            else
            {
                Screen.fullScreen = true;
                fsToggle.SetIsOnWithoutNotify(true);
            }
            return;
        }
        else if (PlayerPrefs.HasKey(key)) 
        {
            float value = PlayerPrefs.GetFloat(key);
            SetVolume(key, value);
            switch (key)
            {
                case "MasterVolume":
                    masterSlider.SetValueWithoutNotify(value);
                    break;
                case "MusicVolume":
                    musicSlider.SetValueWithoutNotify(value);
                    break;
                case "SFXMasterVolume":
                    masterSFXSlider.SetValueWithoutNotify(value);
                    break;
            }  
        }
    }
}
