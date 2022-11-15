using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private SceneLoader sceneLoader;

    public GameObject countdownPanel;


    public string gameName;

    public static AppManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;

        if (SceneManager.GetActiveScene().name == "Basketball")
        {
            gameName = "Basketball";
        }
        else if (SceneManager.GetActiveScene().name == "Mosquito")
        {
            gameName = "Mosquito";
        }
        else if (SceneManager.GetActiveScene().name == "CarRace")
        {
            gameName = "CarRace";
        }
        else if (SceneManager.GetActiveScene().name == "Starship")
        {
            gameName = "Starship";
        }

    }
    void Start()
    {
        SetInitialReferences();
        Timer.RoundEnd += OnRoundEnd;

        ReStart();
    }
    void OnDisable()
    {
        Timer.RoundEnd -= OnRoundEnd;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    protected virtual void SetInitialReferences()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        // Input.simulateMouseWithTouches = false;
    }

    public void StartCountdown()
    {
        SoundManager.Instance.countdown.Play();
        countdownPanel.SetActive(true);
    }


    public void ReStart()
    {
        //Debug.Log("Ω√¿€!!!");
        SoundManager.Instance.countdown.Play();
        countdownPanel.SetActive(true);
    }
    
    public void GlobalLoad(int index)
    {
        sceneLoader.LoadScene(index);
    }

    public void HomeButClick()
    {
        SceneManager.LoadScene("Main");
    }

    protected void OnRoundEnd()
    {
        StopAllCoroutines();
        StartCoroutine(_OnRoundEnd());
    }

    public void OnRoundStart()
    {
        StopAllCoroutines();
        StartCoroutine(_OnRoundStart());
    }

    protected virtual IEnumerator _OnRoundStart()
    {
        yield return UIManager.Instance.OnRoundStart();

        yield return DataManager.Instance.OnRoundStart();

        //AdManager.Instance.ToggleAd(true);
    }

    protected virtual IEnumerator _OnRoundEnd()
    {
        //AdManager.Instance.ToggleAd(false);

        yield return DataManager.Instance.OnRoundEnd();

        yield return UIManager.Instance.OnRoundEnd();
    }
}