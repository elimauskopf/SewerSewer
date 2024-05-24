using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorialButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.CompleteLevel();
    }
}
