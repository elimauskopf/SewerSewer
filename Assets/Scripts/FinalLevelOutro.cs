using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalLevelOutro : MonoBehaviour
{
    public static FinalLevelOutro Instance { get; private set; }

    PlayableDirector _timelineDirector;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _timelineDirector = GetComponent<PlayableDirector>();
    }

    public void OnLevelComplete()
    {
        _timelineDirector.Play();
    }
    public void DeletePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Player);
        for(int i = 0; i < players.Length; i++)
        {
            Destroy(players[i]);
        }
    }
}
