using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    public List<AudioClip> _audioClips = new List<AudioClip>();

    PlayableDirector _timeline;
    AudioSource _audioSource;

    bool _timelinePaused;
    int _narrationClipIndex;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        _timeline = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
        _audioSource = GetComponent<AudioSource>();
        _narrationClipIndex = 0;
        _timelinePaused = false;
    }

    private void Start()
    {
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
    }

    private void Update()
    {
        //if the audio has stopped but the timeline is still running, pause it
        if(_audioSource.isPlaying)
        {
            return;
        }
        else if(_timelinePaused)
        {
            return;
        }
        else
        {
            PauseTimeline();
        }
    }

    public void PlayNextClip()
    {
        Debug.Log("Playing next clip");
        _narrationClipIndex++;
        _audioSource.clip = _audioClips[_narrationClipIndex];
        _audioSource.Play();
        PlayTimeline();
    }

    public void PauseTimeline()//used as an animation event during the tutorial timeline
    {
        _timeline.Pause();
        _timelinePaused=true;
    }

    public void PlayTimeline()
    {
        _timeline.Play();
        _timelinePaused=false;
    }
}
