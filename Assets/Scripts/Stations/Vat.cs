using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vat : StationController
{
    //check to see if the player has the required item to use the station
    protected override bool HandlePlayerItem(PlayerController currentPlayer)
    {
        if (currentPlayer == null)
        {
            return false;
        }

        //if the player is carrying dye
        if (currentPlayer.currentItem.Equals(ItemTypes.Dye))
        {
            SetColor(currentPlayer.currentColor);
            currentPlayer.DropItem();
            return true;
        }
        else if (currentPlayer.currentItem == null)//player isn't carrying anything
        {
            Debug.Log("Player isn't carrying anything");
            return false;
        }

        //check to see if the player is carrying something to be dyed
        foreach (ItemTypes item in itemsRequiredToStart)
        {
            if (currentPlayer.currentItem.Equals(item))//if the player has a required item, add it but don't change the color of the vat
            {
                _currentItemType = currentPlayer.currentItem;
                currentPlayer.DropItem();
                return true;
            }
        }

        return false;
    }

    void SetColor(ColorTypes? newColor)
    {
        _currentColor = newColor;
    }
}
