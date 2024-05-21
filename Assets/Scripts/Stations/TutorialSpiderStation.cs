using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpiderStation : Spider
{
    bool _hasTriggeredNextTutorial;

    protected override void Start()
    {
        base.Start();
        _hasTriggeredNextTutorial = false;
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
            Debug.Log("Spider calling next clip");
            TutorialController.Instance.PlayNextClip();
            _hasTriggeredNextTutorial=true;
        }
    }
}
