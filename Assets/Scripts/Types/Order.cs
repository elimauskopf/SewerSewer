using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order 
{
    public ItemObject dress;
    public ItemObject? ribbon;

    public Order(ItemObject newDress, ItemObject? newRibbon)
    {
        dress = newDress;
        ribbon = newRibbon;
    }

    public Order(ItemObject newDress)
    {
        dress = new ItemObject(ItemTypes.Dress, ColorTypes.White);
        ribbon = null;
    }

    public Order()
    {
        dress = new ItemObject(ItemTypes.Dress, ColorTypes.White);
        ribbon = new ItemObject(ItemTypes.Ribbon, ColorTypes.White);
    }
}
