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
}
