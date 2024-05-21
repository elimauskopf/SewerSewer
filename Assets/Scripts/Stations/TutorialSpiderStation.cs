using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpiderStation : Spider
{
    bool _hasTriggeredNextTutorial;
    bool _hasCollectedSilk;

    protected override void Start()
    {
        base.Start();
        _hasTriggeredNextTutorial = false;
        _hasCollectedSilk = false;
        _isAbleToCharge = false;
        _timer = 0;
    }
    protected override void Update()
    {
        if(!_isAbleToCharge)
        {
            return;
        }
        else if (_timer < timeToComplete)
        {
            _timer += Time.deltaTime;
        }
        else if(!_hasTriggeredNextTutorial)
        {
            TutorialController.Instance.PlayNextClip();
            _hasTriggeredNextTutorial=true;
        }
    }

    public override void CompleteTask()
    {
        base.CompleteTask();
        if(!_hasCollectedSilk)
        {
            _hasCollectedSilk=true;
            TutorialController.Instance.PlayNextClip();
        }
    }
}
