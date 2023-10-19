using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    public AudioClip _theme, _outro;
    AudioSource _audio;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        _audio = GetComponent<AudioSource>();
        _audio.clip = _theme;
        _audio.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Equals(Scenes.Home.ToString()))
        {
            Destroy(gameObject);
            return;
        }
        else if(scene.name.Equals(Scenes.Outro.ToString()))
        {
            _audio.clip = _outro;
            _audio.Play();
        }
        else if(!_audio.clip.Equals(_theme))
        {
            _audio.clip = _theme;
            _audio.Play();
        }
    }
}
