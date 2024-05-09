using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : StationController
{
    SpriteRenderer _dressRenderer;
    SpriteRenderer _ribbonRenderer;

    public List<Sprite> dresses;
    public List<Sprite> ribbons;

    Dictionary<ItemTypes, ColorTypes?> _itemsOnMannequin = new Dictionary<ItemTypes, ColorTypes?>();

    protected override void Awake()
    {
        base.Awake();

        _dressRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _ribbonRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        ClearContents();
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
                AddDress(currentPlayer.currentColor);
                break;
            case ItemTypes.Ribbon:
                AddRibbon(currentPlayer.currentColor);
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
         _chargeBarController?.AddCharge();

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

        _dressRenderer.sprite = dresses[(int)color];
        _dressRenderer.enabled = true;
    }

    void AddRibbon(ColorTypes? color)
    {
        if (color == null)
        {
            return;
        }

        _ribbonRenderer.sprite = ribbons[(int)color];
        _ribbonRenderer.enabled = true;
    }

    void PlayerPickupContents()
    {
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
