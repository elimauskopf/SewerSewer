using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    public static EndLevelUI Instance { get; private set; }
    Animator _animator;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _animator = GetComponent<Animator>();
    }

    public void LevelComplete()
    {
        _animator.SetTrigger(Tags.Moving);
    }
    public void NextLevel()
    {
        _animator.SetTrigger(Tags.Next);
    }
    public void ReturnHome()
    {
        _animator.SetTrigger(Tags.Return);
    }

    //used as event in animation of UI
    public void TriggerNextScene()
    {
        SceneNavigator.Instance.LoadScene();
    }
    //used as event in animation of UI
    public void TriggerLoadHome()
    {
        SceneNavigator.Instance.LoadHome();
    }
}
