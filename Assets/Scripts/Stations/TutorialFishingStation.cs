using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFishingStation : FishingStation
{
    bool _hasBeenInitiated;
    bool _hasGivenPlayerFish;

    private void Start()
    {
        _hasBeenInitiated = false;
        _hasGivenPlayerFish = false;
    }
    public override bool Initiate(GameObject player)
    {
        PlayerController _assignedPlayer = player.GetComponent<PlayerController>();

        if (!_hasBeenInitiated)
        {
            TutorialController.Instance.PlayNextClip();
            _hasBeenInitiated = true;
        }

        //REMOVE ONCE STATION IS WORKING
        else if(!_hasGivenPlayerFish)
        {
            _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
            _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
            TutorialController.Instance.PlayNextClip();
            _hasGivenPlayerFish = true;
        }
        //
       
        base.Initiate(player);
        return false;
    }

    public override void CompleteTask()
    {
        if(!_hasGivenPlayerFish)
        {
            TutorialController.Instance.PlayNextClip();
            _hasGivenPlayerFish = true;
        }
        base.CompleteTask();
    }
}
