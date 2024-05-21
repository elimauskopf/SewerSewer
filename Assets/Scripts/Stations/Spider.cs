using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : StationController
{
    protected virtual void Start()
    {
        _timer = timeToComplete;
        _chargeBarController.UpdateChargeBar();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _animator.SetBool(Tags.InRange, true);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        _animator.SetBool(Tags.InRange, false);
    }

    public override void CompleteTask()
    {
        _currentColor = ColorTypes.White;
        if (isPassive)
        {
            _timer = 0;
            _chargeBarController.HideChargeBar();
        }
        else
        {
            _animator.SetBool(Tags.Moving, false);
        }

        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        _currentItemType = ItemTypes.None;
        _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
        _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }
}
