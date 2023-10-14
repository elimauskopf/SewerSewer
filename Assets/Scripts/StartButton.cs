using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public Animator boatAnimator;
    public Animator blackFade;
    public void StartGame()
    {
        SceneNavigator.Instance.StartGame();
        gameObject.SetActive(false);
        boatAnimator?.SetTrigger(Tags.Moving);
        blackFade?.SetTrigger(Tags.Moving);
        gameObject.SetActive(false);
    }
}
