using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public static SceneNavigator Instance { get; private set; }

    public Scenes sceneToLoad;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
    }

    public void StartGame()
    {
        StartCoroutine(WaitToStart());
    }

    public void LoadHome()
    {
        SceneManager.LoadScene(Scenes.Home.ToString());
    }

    IEnumerator WaitToStart()
    {
        float timer = 0;
        while (timer < 3)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad.ToString());
    }
}
