     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStation : StationController
{
    [SerializeField]
    protected Sprite _blueCircle, _redCircle;

    // Does station use circle minigame and local variables for scripts
    public bool isFishingGame;
    private ButtonMiniGame buttonMiniGame;

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


        buttonMiniGame = transform.Find("ButtonGame").GetComponent<ButtonMiniGame>();

    }




    //used when player interacts with station
    public override bool Initiate(GameObject player)
    {
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

            buttonMiniGame.Show();
            buttonMiniGame.engaged = true;


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



            buttonMiniGame.EndInteraction();

        }

        AssignUI();
    }

    public override void WorkStation()
    {

        if (buttonMiniGame.PlayerPressedButton())
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

        buttonMiniGame.EndInteraction();


        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        _currentItemType = ItemTypes.None;
        _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
        _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }

    protected override void AssignUI()
    {
        _uiButton?.SetActive(true);
        _iconObject?.SetActive(true);

        if (stationInUse || _isAbleToCharge)
        {
            _uiRenderer.sprite = _redCircle;
            _iconObject?.SetActive(false);

        }
        else if (playersByStation == 0)
        {
            _uiButton?.SetActive(false);
            _iconObject?.SetActive(false);
            _chargeBarController?.HideChargeBar();


        }
        else
        {
            _uiRenderer.sprite = _blueCircle;
            _iconObject?.SetActive(true);
        }
    }

}
