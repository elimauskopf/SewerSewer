using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoat : Boat
{
    bool _hasTriggeredNextTutorial;

    private void Start()
    {
        _hasTriggeredNextTutorial = false;
    }
    protected override void StartDelivery()
    {
        if(!_hasTriggeredNextTutorial)
        {
            TutorialController.Instance.PlayNextClip();
            _hasTriggeredNextTutorial = true;
        }
        base.StartDelivery();
    }
}
