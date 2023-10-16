using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance { get; private set; }

    public GameObject shoe;
    public float timeForOrderToMoveForward;

    List<Transform> positionsInLine = new List<Transform>();
    List<OrderInLine> ordersInLine = new List<OrderInLine>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        foreach(Transform child in transform)
        {
            positionsInLine.Add(child);
        }
    }

    public void AddOrder()
    {
        //instantiate the shoe at the last position in line (off screen)
        ordersInLine.Add((Instantiate(shoe, positionsInLine[positionsInLine.Count - 1].position, Quaternion.identity)).GetComponent<OrderInLine>());
        int positionInLine = ordersInLine.Count - 1;
        ordersInLine[positionInLine].MoveTowards(positionsInLine[positionInLine].position);
    }

    public void CompleteOrder()
    {
        if(ordersInLine.Count == 0)
        {
            return;
        }

        for(int i = 1; i < ordersInLine.Count; i++)
        {
            ordersInLine[i].MoveTowards(positionsInLine[i - 1].position);
        }
        OrderInLine completedOrder = ordersInLine[0];
        ordersInLine.Remove(ordersInLine[0]);
        completedOrder.CompleteOrder();
        //tell each shoe which position to move towards
    }
}
