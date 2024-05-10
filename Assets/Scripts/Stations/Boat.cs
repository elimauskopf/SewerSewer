using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : StationController
{
    private LineManager lineManager;
    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        lineManager = GameObject.Find("LineManager").GetComponent<LineManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    private bool CheckOrder(PlayerController playerController)
    {
        for (int i = 1; i < lineManager.customer.Count; i++)
        {
            // Is the dress the same color
            if (lineManager.customer[i].order.dress.ColorType == playerController.currentOrder.dress.ColorType)
            {
                // Does it require a ribbon
                if (gameManager.isRegionThree && lineManager.customer[i].order.ribbon.ItemType != ItemTypes.None)
                {
                    // Is the ribbon the same color
                    if (playerController.currentOrder.ribbon.ColorType  == lineManager.customer[i].order.ribbon.ColorType)
                    {
                        return true;
                    }
                } else
                {
                    return true;
                }
            }
        }

        return false;
    }

    public override bool Initiate(GameObject player)
    {
        if (stationInUse)
        {
            Debug.Log("Station is in use");
            return false;
        }

        PlayerController currentPlayer = player.GetComponent<PlayerController>();
        if (HandlePlayerItem(currentPlayer) && CheckOrder(currentPlayer))
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
