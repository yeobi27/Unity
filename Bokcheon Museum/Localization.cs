using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Localization : MonoBehaviour
{
    public static Localization Instance { get; private set; }

    PlayerPrefLocaleSelector playerPref;
    LocalizationSettings settings;
    private void Awake()
    {
        if (Instance != null) { Destroy(this); }
        else { Instance = this; }
    }
    IEnumerator Start()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;
    }

    public void SetLocale(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public string GetLocale()
    {
        return LocalizationSettings.SelectedLocale.LocaleName;
    }
}
