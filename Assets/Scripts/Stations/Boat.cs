using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boat : StationController
{
    private LineManager lineManager;
    private GameManager gameManager;
    private int customerIndex;
    protected override void Awake()
    {
        base.Awake();
        lineManager = GameObject.Find("LineManager").GetComponent<LineManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public override void CompleteTask()
    {
        Debug.Log("Completing task");
        GameManager.Instance?.CompleteOrder(customerIndex);
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
        print("PLAYER ORDER, Dress: " + playerController.currentOrder.dress.ColorType + " Ribbon: " + playerController.currentOrder.ribbon.ColorType);
        //print(lineManager.customer.Count);
        

        for (int i = 0; i < lineManager.customer.Count; i++)
        {
            print("CURRENT ORDER,  Dress: " + lineManager.customer[i].order.dress.ColorType + " Ribbon: " + lineManager.customer[i].order.ribbon.ColorType);
            // Is the dress the same color
            if (lineManager.customer[i].order.dress.ColorType == playerController.currentOrder.dress.ColorType)
            {
                // Does it require a ribbon
                if (gameManager.isRegionThree && lineManager.customer[i].order.ribbon.ItemType != ItemTypes.None)
                {
                    // Is the ribbon the same color
                    if (playerController.currentOrder.ribbon.ColorType  == lineManager.customer[i].order.ribbon.ColorType)
                    {
                        customerIndex = i;
                        return true;
                    }
                } else
                {
                    customerIndex = i;
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
        if ( SceneManager.GetActiveScene().name.Contains("Tutorial") && HandlePlayerItem(currentPlayer))
        {
            StartDelivery();
            return true;
        }
        
        if (HandlePlayerItem(currentPlayer) && CheckOrder(currentPlayer))
        {
            StartDelivery();
            print("here");
            return true;
        }
        else
        {
            return false;
        }
    }
}
