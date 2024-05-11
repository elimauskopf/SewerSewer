using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager Instance { get; private set; }

    public GameObject shoe;
    public float timeForOrderToMoveForward;

    List<Transform> positionsInLine = new List<Transform>();
    public List<Customer> customer = new List<Customer>();

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

    public void AddOrder(Order newOrder)
    {

        Customer newCustomer = Instantiate(shoe, positionsInLine[positionsInLine.Count - 1].position, Quaternion.identity).GetComponent<Customer>();
        print("LM: " + newOrder.dress.ColorType);
        newCustomer.PopulateOrder(newOrder);
        //instantiate the shoe at the last position in line (off screen)
        customer.Add(newCustomer);
        int positionInLine = customer.Count - 1;
        if(positionInLine < positionsInLine.Count)
        {
            customer[positionInLine].MoveTowards(positionsInLine[positionInLine].position);
        }
    }

    public void CompleteOrder(int customerIndex)
    {
        if(customer.Count == 0)
        {
            return;
        }

       

        for (int i = customerIndex + 1; i < customer.Count; i++)
        {
            if(i >= positionsInLine.Count)
            {
                continue;
            }
            customer[i].MoveTowards(positionsInLine[i-1].position);
        }

        Customer completedOrder = customer[customerIndex];
        customer.Remove(customer[customerIndex]);
        completedOrder.CompleteOrder();
        //tell each shoe which position to move towards
    }
}
