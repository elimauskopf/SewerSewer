using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : StationController
{
    SpriteRenderer _dressRenderer;
    SpriteRenderer _ribbonRenderer;

    public List<Sprite> dresses;
    public List<Sprite> ribbons;


    private Order currentOrder;
    Dictionary<ItemTypes, ColorTypes?> _itemsOnMannequin = new Dictionary<ItemTypes, ColorTypes?>();

    bool hasDress;
    bool hasRibbon;
    protected override void Awake()
    {
        base.Awake();


        currentOrder = new Order();
        _dressRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _ribbonRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        ClearContents();
    }

    public override bool Initiate(GameObject player)
    {
        if (hasRibbon && hasDress)
        {
            Debug.Log("Station is in use");
            player.GetComponent<PlayerController>().currentOrder.dress = currentOrder.dress;
            player.GetComponent<PlayerController>().currentOrder.ribbon = currentOrder.ribbon;
            return false;
           

        }


        PlayerController currentPlayer = player.GetComponent<PlayerController>();
        print(currentPlayer.currentStation);



        HandlePlayerItem(currentPlayer);


        return false;
    }
  
    protected override bool HandlePlayerItem(PlayerController currentPlayer)
    {
        if (currentPlayer == null)
        {
            return false;
        }

        //if player is not carrying anything, give content of mannequin
        //
        if (currentPlayer.currentItem.Equals(ItemTypes.None))//if the player doesn't have anything 
        {
            Debug.Log("Player not carrying anything");
            return false;
            //give them the contents
        }

        switch(currentPlayer.currentItem)
        {
            case ItemTypes.Dress:
                if (currentPlayer.currentColor != null)
                {
                    AddDress(currentPlayer.currentColor);
                } else
                {
                    AddDress(ColorTypes.White);
                }
                hasDress = true;
                break;
            case ItemTypes.Ribbon:
                if (currentPlayer.currentColor != null)
                {
                    AddRibbon(currentPlayer.currentColor);
                }
                else
                {
                    AddRibbon(ColorTypes.White);
                }
                hasRibbon = true;
                break;
            default:
                return false;
        }

        _itemsOnMannequin.Add(currentPlayer.currentItem, currentPlayer.currentColor);
        currentPlayer.DropItem();
        return true;
    }

    public override void WorkStation()
    {
         //_chargeBarController?.AddCharge();

        if (_chargeBarController && _chargeBarController.percentReloaded >= 1)
        {
            // Player finished station
            CompleteTask();
        }
    }

    protected override void CompleteTask()
    {
        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        _currentItemType = ItemTypes.None;
        _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
        _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }

    void AddDress(ColorTypes? color)
    {
        Debug.Log("Adding dress to mannequin");
        if(color == null)
        {
            Debug.Log("no color assigned to dress");
            return;
        }

        currentOrder.dress = new ItemObject(ItemTypes.Dress, color);
        _dressRenderer.sprite = dresses[(int)color];
        _dressRenderer.enabled = true;
    }

    void AddRibbon(ColorTypes? color)
    {/*
        if (color == null)
        {
            return;
        }*/

        currentOrder.ribbon = new ItemObject(ItemTypes.Ribbon, color);
        _ribbonRenderer.sprite = ribbons[(int)color];
        _ribbonRenderer.enabled = true;
    }

    public void PlayerPickupContents(PlayerController currentPlayer)
    {
        if (hasDress)
        {
            currentPlayer.currentOrder.dress = currentOrder.dress;

            currentPlayer.currentItem = ItemTypes.Dress;
            currentPlayer.currentColor = currentOrder.dress.ColorType;
            currentPlayer.AssignItem(ItemTypes.Dress, currentOrder.dress.ColorType);
        }

        if (hasRibbon)
        {
            currentPlayer.currentOrder.ribbon = currentOrder.ribbon;
            currentPlayer.currentRibbon = currentOrder.ribbon;

            // Did player just pick up a dress too
            if (currentPlayer.currentItem != ItemTypes.Dress)
            {
                currentPlayer.AssignItem(ItemTypes.Ribbon, currentOrder.ribbon.ColorType);
            } else
            {
                currentPlayer.AssignRibbon();
            }
         

           
            //currentPlayer.currentColor = currentOrder.dress.ColorType;
        }

        //have to add dress and all ornamentation to player
        ClearContents();
    }

    void ClearContents()
    {
        _ribbonRenderer.sprite = null;
        _ribbonRenderer.enabled = false;

        _dressRenderer.sprite = null;
        _dressRenderer.enabled = false;


      
        _itemsOnMannequin.Clear();
    }
    
}
