using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : StationController
{
    private LineManager lineManager;

    protected override void Awake()
    {
        base.Awake();
        lineManager = GameObject.Find("Roof").transform.GetChild(0).GetComponent<LineManager>();
    }
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

    /*private bool CheckOrder(PlayerController playerController) 
    {
        for (int i = 1; i < lineManager.customer.Count; i++)
        {
            if ()
        }
    }*/

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
