using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Vat : StationController
{

    private bool itemInVat;

    private void Start()
    {
        AssignUI();
    }


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
            else if (itemInVat)
            {
                _assignedPlayer = player;
                CompleteTask();
            }
        }
        return false;


    }
    //check to see if the player has the required item to use the station
    protected override bool HandlePlayerItem(PlayerController currentPlayer)
    {
        print(currentPlayer);
        if (currentPlayer == null)
        {
            return false;
        }

        //if the player is carrying dye
        if (currentPlayer.currentItem.Equals(ItemTypes.Dye))
        {
            _chargeBarController.ResetChargeBar();
            SetColor(currentPlayer.currentColor);
            currentPlayer.DropItem();
            _currentItemType = ItemTypes.None;
            itemInVat = false;
            AssignUI();
            //dyeInVat = true;
            return false;
        }
        else if (currentPlayer.currentItem.Equals(ItemTypes.None))//player isn't carrying anything
        {
            Debug.Log("Player isn't carrying anything");
            return false;
        }

        //check to see if the player is carrying something to be dyed
        foreach (ItemTypes item in itemsRequiredToStart)
        {

            if (currentPlayer.currentItem.Equals(item)
                && currentPlayer.currentColor != _currentColor)//if the player has a required item, add it but don't change the color of the vat
            {

                _currentItemType = currentPlayer.currentItem;
                currentPlayer.DropItem();
                itemInVat = true;
                return true;
            }
        }

        return false;
    }

    public override void WorkStation()
    {

    }

    protected override void CompleteTask()
    {
        

            _chargeBarController.ResetChargeBar();
            _isAbleToCharge = false;
            _timer = 0;
            _chargeBarController.HideChargeBar();
            _assignedPlayer?.GetComponent<PlayerController>().AssignItem(_currentItemType, _currentColor);
            _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
            _currentItemType = ItemTypes.None;
            SetColor(ColorTypes.White);
            AssignUI();
        

    }

    void SetColor(ColorTypes? newColor)
    {
        _currentColor = newColor;
        foreach(ColorTypes type in Enum.GetValues(typeof(ColorTypes))) {
            _animator.ResetTrigger(type.ToString());
        }
        _animator.SetTrigger(newColor.ToString());
    }
}
