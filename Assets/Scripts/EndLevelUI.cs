using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevelUI : MonoBehaviour
{
    public static EndLevelUI Instance { get; private set; }

    public TMP_Text _rightButtonText;
    public TMP_Text timeLeftText;

    Animator _animator;
    TMP_Text _messageText;

    bool _levelLost;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        _levelLost = false;
        _animator = GetComponent<Animator>();
        _messageText = transform.GetChild(0).GetComponent<TMP_Text>();
        _messageText.enabled = false;
        timeLeftText = transform.Find("TimeLeft/Text").GetComponent<TMP_Text>();
    }

    public void LevelComplete()
    {
        _animator.SetTrigger(Tags.Moving);
        _messageText.text = "The king is satisfied.\nYou may live.";
        _messageText.enabled = true;
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
        if(_levelLost)
        {
            SceneNavigator.Instance.LoadTutorial();
        }
        else
        {
            SceneNavigator.Instance.LoadScene();
        }
    }
    //used as event in animation of UI
    public void TriggerLoadHome()
    {
        SceneNavigator.Instance.LoadHome();
    }

    public void OnLevelLost()
    {
        _levelLost = true;
        _rightButtonText.text = "Try Again";
        _messageText.text = "The king is upset.\nBeg for mercy.";
      
        _messageText.enabled = true;
        _animator.SetTrigger(Tags.Moving);
    }
}
