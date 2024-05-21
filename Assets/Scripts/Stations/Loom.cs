using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loom : StationController
{
    private TimerMinigame timerMiniGame;

    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        _uiButton = transform.Find(Tags.UI)?.gameObject;
        _uiRenderer = _uiButton?.GetComponent<SpriteRenderer>();

        _chargeBarObject = transform.Find(Tags.ChargeBar)?.gameObject;
        _chargeBarController = _chargeBarObject?.GetComponent<ChargeBarController>();

        _iconObject = transform.Find(Tags.Icon)?.gameObject;
        _iconRenderer = _iconObject?.GetComponent<SpriteRenderer>();
        if (itemsRequiredToStart.Count > 0 && _iconRenderer)
        {
            _iconRenderer.sprite = itemRequiredIcon;
        }

        _uiButton?.SetActive(false);
        _iconObject?.SetActive(false);

        timerMiniGame = transform.Find("TimerGame").GetComponent<TimerMinigame>();


    }


    //used when player interacts with station
    public override bool Initiate(GameObject player)
    {
        Debug.Log("Player engaging with loom");
        if (stationInUse)
        {
            Debug.Log("Station is in use");
            return false;
        }


        PlayerController currentPlayer = player.GetComponent<PlayerController>();
        print(currentPlayer.currentStation);

        if (!currentPlayer.currentStation) return false;


        if (isPassive)
        {
            if (_timer == 0)//station is not active, waiting for input
            {
                if (HandlePlayerItem(currentPlayer))
                {
                    _isAbleToCharge = true;
                    _chargeBarController?.StartChargeBar();
                }
            }
            else if (_timer >= timeToComplete)//station is ready
            {

                if (HandlePlayerItem(currentPlayer)) // player has fish and spider is done
                {
                    _assignedPlayer = player;
                    CompleteTask();
                    _isAbleToCharge = true;
                    _chargeBarController?.StartChargeBar();

                }
                else
                {
                    _assignedPlayer = player;
                    CompleteTask();
                }
            }
            return false;
        }
        else //if station is active (not passive)
        {
            if (!_isAbleToCharge && !HandlePlayerItem(currentPlayer))
            {
                return false;
            }

            _assignedPlayer = player;
            stationInUse = true;
            _chargeBarController?.StartChargeBar();
            _animator.SetBool(Tags.Moving, true);

            if (!_isAbleToCharge)
            {
                _isAbleToCharge = true;
                Debug.Log("station in use");
            }


            timerMiniGame.Show();
            timerMiniGame.engaged = true;

            AssignUI();
            return true;
        }
    }


    public override void Disengage(GameObject player)
    {
        if (!stationInUse)
        {
            return;
        }

        if (_assignedPlayer == player)
        {
            print("leave");
            _assignedPlayer = null;
            stationInUse = false;


            timerMiniGame.EndInteraction();

        }

        AssignUI();
    }

    public override void WorkStation()
    {
        if (!timerMiniGame.engaged) return;

        if (timerMiniGame.WasButtonPressedOnTime())
        {
            _chargeBarController?.AddCharge();
        }



        if (_chargeBarController && _chargeBarController.percentReloaded >= 1)
        {
            // Player finished station
            CompleteTask();
        }
    }

    public override void CompleteTask()
    {
        if (isPassive)
        {
            _timer = 0;
            _chargeBarController.HideChargeBar();
        }
        else
        {
            _animator.SetBool(Tags.Moving, false);
        }

        timerMiniGame.EndInteraction();

        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        _currentItemType = ItemTypes.None;
        _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
        _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }


 
}
