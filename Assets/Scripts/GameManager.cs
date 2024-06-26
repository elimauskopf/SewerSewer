using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    TMP_Text _timerText;

    [SerializeField] List<AudioClip> _orderCompleteClips;
    [SerializeField] AudioClip _levelWonClip, _levelLostClip;
    AudioSource _orderCompleteAudio;
    AudioSource _levelEndAudio;

    public bool isRegionOne;
    public bool isRegionTwo;
    public bool isRegionThree;
    public bool levelCompleted;
    public List<bool> pendingOrders;
    int ordersComplete;
    int currentLevel;
    [SerializeField]
    int totalOrdersThisLevel;

    //the amount of orders in the easiest level, used as the order number floor
    int _lowestOrderNumber = 1;
    float _secondsPerLevel = (3 * 60f);
    float _levelTimer;
    float _orderTimer;
    float _timeUntilNextOrder;

    bool _timerPaused;

    // Order generation
    private string[] colors = { "White", "Red", "Green", "Yellow" };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }


        _orderCompleteAudio = transform.Find(Tags.OrderCompleteAudio)?.GetComponent<AudioSource>();
        _levelEndAudio = transform.Find(Tags.LevelEndAudio)?.GetComponent<AudioSource>();
        _levelTimer = _secondsPerLevel;
        _timerText = GameObject.FindWithTag(Tags.Timer)?.GetComponent<TMP_Text>();
        _timerPaused = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Tutorial") || !_timerPaused)
        {
            if (_levelTimer > 0)
            {
                _levelTimer -= Time.deltaTime;
                CalculateTimer();
            }
        }
        else
        {
            return;
        }

        //if all the orders for the level have entered the scene, don't add any more
        if (ordersComplete + pendingOrders.Count >= totalOrdersThisLevel)
        {
            return;
        }

        _orderTimer += Time.deltaTime;
        if (_orderTimer > _timeUntilNextOrder)
        {
            AddOrder();
            _timeUntilNextOrder = UnityEngine.Random.Range(3f, 8f);
            _orderTimer = 0;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Equals(Scenes.Home.ToString()))
        {
            //Destroy(gameObject);
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

        levelCompleted = false;
        CalculateOrders();
    }

    void AddOrder()
    {
        pendingOrders.Add(true);
        LineManager.Instance.AddOrder(GenerateOrder());
        //also need to add shoe animation walking across the scene and waiting
    }

    Order GenerateOrder()
    {


        if (isRegionOne || isRegionTwo)
        {
            return new Order(GenerateItem(ItemTypes.Dress), new ItemObject(ItemTypes.None, ColorTypes.None));
        }
        else if (isRegionThree)
        {
            return new Order(GenerateItem(ItemTypes.Dress), GenerateItem(ItemTypes.Ribbon));
        }

        return null;
    }

    ItemObject GenerateItem(ItemTypes itemType)
    {
        ColorTypes colorType;

        if (isRegionOne)
        {
            colorType = ColorTypes.White;
        }
        else
        {

            colorType = RandomColor();
        }


        return new ItemObject(itemType, colorType);
    }

    public ColorTypes RandomColor()
    {
 

        switch (Random.Range(0, 4))
        {
            case 0:
                return ColorTypes.White;
            case 1:
                return ColorTypes.Green;
            case 2:
                return ColorTypes.Red;
            case 3:
                return ColorTypes.Yellow;
            default:
                return ColorTypes.White;

        }

    }
    public void CompleteOrder(int customerIndex)
    {
        if (_levelTimer <= 0)
        {
            return;
        }
        if(_timerPaused)
        {
            return;
        }

        int orderAudio = Random.Range(0, _orderCompleteClips.Count - 1);
        _orderCompleteAudio.clip = _orderCompleteClips[orderAudio];
        _orderCompleteAudio.Play();

        ordersComplete++;
        pendingOrders.Remove(true);
        LineManager.Instance.CompleteOrder(customerIndex);

        if (ordersComplete == totalOrdersThisLevel)
        {
            CompleteLevel();
        }
    }

    public void CompleteLevel()
    {
        Save(_levelTimer);

        if (SceneManager.GetActiveScene().name.Equals(Scenes.Level_3_5.ToString()))
        {
            FinalLevelOutro.Instance.OnLevelComplete();
        }
        else if (EndLevelUI.Instance != null)
        {
            EndLevelUI.Instance.LevelComplete();
            EndLevelUI.Instance.HandleStars(_levelTimer);
            levelCompleted = true;
            _levelEndAudio.clip = _levelWonClip;
            _levelEndAudio.Play();
        }
        else
        {
            SceneNavigator.Instance.LoadScene();
        }
    }

    void CalculateOrders()
    {
        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            totalOrdersThisLevel = 2;
        }
        else
        {
            //totalOrdersThisLevel = _lowestOrderNumber + (currentLevel * 2);
            //totalOrdersThisLevel = 1;

        }
    }

    void CalculateTimer()
    {

        if (levelCompleted) return;

        if (_levelTimer <= 0)
        {
            LoseLevel();
        }
        int minutes = Mathf.FloorToInt(_levelTimer / 60);
        int seconds = Mathf.FloorToInt(_levelTimer % 60);

        _timerText.text = "Time left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void LoseLevel()
    {
        _levelEndAudio.clip = _levelLostClip;
        _levelEndAudio.Play();
        EndLevelUI.Instance.OnLevelLost();
    }

    public void PauseTimer()
    {
        _timerPaused = true;
        _timerText.enabled = false;
    }

    public void ResumeTimer()
    {
        _timerPaused = false;
        _timerText.enabled = true;
    }

    void Save(float timer)
    {
        ES3.Save(Tags.TimeRemaining + SceneManager.GetActiveScene().name, timer);
    }
}
