using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : StationController
{

    protected override void CompleteTask()
    {
        Debug.Log("Completing task");
        GameManager.Instance?.CompleteOrder();
    }

    void StartDelivery()
    {
        stationInUse = true;
        _animator.SetTrigger(Tags.Moving);
        _uiButton?.SetActive(false);
        _iconObject.SetActive(false);
    }

    public void BoatReturned()
    {
        stationInUse = false;
        if (_playerInRange)
        {
            _uiButton?.SetActive(true);
            _iconObject?.SetActive(true);
        }
    }

    public override bool Initiate(GameObject player)
    {
        if (stationInUse)
        {
            Debug.Log("Station is in use");
            return false;
        }

        PlayerController currentPlayer = player.GetComponent<PlayerController>();
        if (HandlePlayerItem(currentPlayer))
        {
            StartDelivery();
            return true;
        }
        else
        {
            return false;
        }
    }
}
