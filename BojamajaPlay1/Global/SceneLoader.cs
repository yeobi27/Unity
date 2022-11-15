using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private List<string> scenes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        scenes = new List<string>();

        Reset();

        foreach (var s in scenes) Debug.Log(s);
    }

    void Reset()
    {
        scenes.Clear();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            scenes.Add(GetSceneNameByBuildIndex(i));
    }
    private string GetSceneNameByBuildIndex(int num)
    {
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(num);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);

        return sceneName;
    }

    public void LoadScene(int index = -1)
    {
        if (scenes.Count > 0)
        {
            if (index == -1)
            {
                index = Random.Range(0, scenes.Count);
                SceneManager.LoadScene(scenes[index]);

                scenes.RemoveAt(index);
            }
            else if (index == 0)
            {
                Reset();
                SceneManager.LoadScene(0);
            }
            else
            {
                scenes.RemoveAt(index - 1);
                SceneManager.LoadScene(index);
            }
        }
        else
        {
            Reset();
            SceneManager.LoadScene(0);
        }

        foreach (var s in scenes) Debug.Log(s);
    }
}
