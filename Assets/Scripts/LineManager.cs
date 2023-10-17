using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance { get; private set; }

    public GameObject shoe;
    public float timeForOrderToMoveForward;

    List<Transform> positionsInLine = new List<Transform>();
    List<Customer> customer = new List<Customer>();

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
        customer.Add((Instantiate(shoe, positionsInLine[positionsInLine.Count - 1].position, Quaternion.identity)).GetComponent<Customer>());
        int positionInLine = customer.Count - 1;
        customer[positionInLine].MoveTowards(positionsInLine[positionInLine].position);
    }

    public void CompleteOrder()
    {
        if(customer.Count == 0)
        {
            return;
        }

        for(int i = 1; i < customer.Count; i++)
        {
            customer[i].MoveTowards(positionsInLine[i - 1].position);
        }
        Customer completedOrder = customer[0];
        customer.Remove(customer[0]);
        completedOrder.CompleteOrder();
        //tell each shoe which position to move towards
    }
}
