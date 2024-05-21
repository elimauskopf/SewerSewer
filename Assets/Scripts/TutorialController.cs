using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    public List<AudioClip> _audioClips = new List<AudioClip>();
    List<PlayableDirector> _timelines = new List<PlayableDirector>();

    PlayableDirector _currentTimeline;

    AudioSource _audioSource;

    bool _timelinePaused;
    int _timelineIndex;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        foreach(Transform child in transform)
        {
            _timelines.Add(child.GetComponent<PlayableDirector>());
        }
        _audioSource = GetComponent<AudioSource>();
        _timelineIndex = 0;
        _timelinePaused = false;
        _currentTimeline = _timelines[_timelineIndex];
    }

    private void Start()
    {
        _audioSource.clip = _audioClips[0];
        _audioSource.Play();
    }

   /* private void Update()
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
    }*/

    public void PlayNextClip()
    {
        Debug.Log("Playing next clip");
        _timelineIndex++;
        _audioSource.clip = _audioClips[_timelineIndex];
        _currentTimeline.Pause();
        _audioSource.Play();
        PlayTimeline();
    }

    public void PauseTimeline()//used as an animation event during the tutorial timeline
    {
        _currentTimeline.Pause();
        _timelinePaused = true;
    }

    public void PlayTimeline()
    {
        _currentTimeline = _timelines[_timelineIndex];
        _currentTimeline.Play();
        _timelinePaused = false;
    }
}
