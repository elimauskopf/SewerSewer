using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    TMP_Text _timerText;

    public List<bool> pendingOrders;
    int ordersComplete;
    int currentLevel;
    int totalOrdersThisLevel;

    //the amount of orders in the easiest level, used as the order number floor
    int _lowestOrderNumber = 5;
    float _secondsPerLevel = (3*60f);
    float _levelTimer;
    float _orderTimer;
    float _timeUntilNextOrder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        _levelTimer = _secondsPerLevel;
        _timerText = transform.Find(Tags.Timer)?.GetComponent<TMP_Text>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        //if all the orders for the level have entered the scene, don't add any more
        if(ordersComplete + pendingOrders.Count >= totalOrdersThisLevel)
        {
            return;
        }

        if(!SceneManager.GetActiveScene().name.Equals("Tutorial"))
        {
            _levelTimer -= Time.deltaTime;
            CalculateTimer();
        }
        else
        {
            CalculateTimer();
        }

        _orderTimer += Time.deltaTime;
        if(_orderTimer > _timeUntilNextOrder)
        {
            AddOrder();
            _timeUntilNextOrder = UnityEngine.Random.Range(3f, 8f);
            _orderTimer = 0;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName.Equals(Scenes.Home.ToString()))
        {
            Destroy(gameObject);
        }

        ordersComplete = 0;
        _levelTimer = _secondsPerLevel;

        // Parse the level number from the scene name
        if (int.TryParse(sceneName.Replace("Level_", ""), out int level))
        {
            currentLevel = level;
        }
        else
        {
            currentLevel = 0;
        }
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

        if(ordersComplete == totalOrdersThisLevel)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        if(SceneManager.GetActiveScene().name.Equals(Scenes.Level_5.ToString()))
        {
            FinalLevelOutro.Instance.OnLevelComplete();
        }
        else if (EndLevelUI.Instance != null)
        {
            EndLevelUI.Instance.LevelComplete();
        }
        else
        {
            SceneNavigator.Instance.LoadScene();
        }
    }

    void CalculateOrders()
    {
        if(SceneManager.GetActiveScene().name.Equals(Scenes.Tutorial.ToString()))
        {
            totalOrdersThisLevel = 2;
        }
        else
        {
            totalOrdersThisLevel = _lowestOrderNumber + (currentLevel*2);
        }
    }

    void CalculateTimer()
    {
        int minutes = Mathf.FloorToInt(_levelTimer / 60);
        int seconds = Mathf.FloorToInt(_levelTimer % 60);

        _timerText.text = "Time left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
