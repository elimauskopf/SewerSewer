using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<bool> pendingOrders;
    int ordersComplete;
    int currentLevel;
    int totalOrdersThisLevel;

    //the amount of orders in the easiest level, used as the order number floor
    int lowestOrderNumber = 3;
    float secondsPerLevel = (3*60f);
    float timer;
    float timeUntilNextOrder;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        //used to test order system, delete in final
        if(Input.GetMouseButtonDown(0))
        {
            CompleteOrder();
        }
        //if all the orders for the level have entered the scene, don't add any more
        if(ordersComplete + pendingOrders.Count >= totalOrdersThisLevel)
        {
            return;
        }

        timer += Time.deltaTime;
        if(timer > timeUntilNextOrder)
        {
            AddOrder();
            timeUntilNextOrder = Random.Range(5f, secondsPerLevel / totalOrdersThisLevel);
            timer = 0;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CalculateOrders();
    }

    void AddOrder()
    {
        pendingOrders.Add(true);
        LineManager.Instance.AddOrder();
        //also need to add shoe animation walking across the scene and waiting
    }

    public void CompleteOrder()
    {
        ordersComplete++;
        pendingOrders.Remove(true);
        LineManager.Instance.CompleteOrder();
    }

    void NextLevel()
    {
        currentLevel++;
    }

    void CalculateOrders()
    {
        totalOrdersThisLevel = lowestOrderNumber + currentLevel;
    }
}
