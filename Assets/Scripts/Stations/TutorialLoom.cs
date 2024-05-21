using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLoom : Loom
{
    bool _hasTriggeredNextTutorial;

    private void Start()
    {
        _hasTriggeredNextTutorial = false;
    }
    public override void CompleteTask()
    {
        if(!_hasTriggeredNextTutorial)
        {
            TutorialController.Instance.PlayNextClip();
            _hasTriggeredNextTutorial = true;
        }
        base.CompleteTask();
    }
}
