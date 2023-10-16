using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : StationController
{
    protected override void CompleteTask()
    {
        //trigger animation
        GameManager.Instance?.CompleteOrder();
    }
}
