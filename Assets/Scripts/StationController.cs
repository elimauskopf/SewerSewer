using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    protected Animator _animator;
    protected GameObject _ui;
    protected GameObject _chargeBarObject;
    protected ChargeBarController _chargeBarController;

    public delegate void StationAction(GameObject player);
    public static event StationAction OnPlayerNearStation;
    public static event StationAction OnPlayerExitStation;

    // Player currently controlling station
    private GameObject _assignedPlayer;
    private int playersByStation;

    //does the station recharge on its own or require player participation to charge
    public bool isPassive;

    //how long does it take the station to charge up
    public float timeToComplete;

    //what does the station give the player when the task is complete (example: spiders give silk)
    //(data type might change from GameObject to something custom)
    public GameObject itemOnCompletion;

    //what does the station require from the player to activate (example: loom requires spider silk)
    public GameObject itemRequiredToStart;

    protected bool _playerInRange;
    protected float _timer;
    protected bool _isReady;
    protected bool _stationInUse;

    public float Timer { get { return _timer; } }

    // Connected player

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _ui = transform.Find(Tags.UI)?.gameObject;

        _chargeBarObject = transform.Find(Tags.ChargeBar)?.gameObject;
        _chargeBarController = _chargeBarObject?.GetComponent<ChargeBarController>();

        _ui?.SetActive(false);
    }

    protected virtual void Update()
    {
        if (_timer < timeToComplete && isPassive)
        {
            _timer += Time.deltaTime;
        }
        //if player presses action button
        //call InteractWithStation()
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag(Tags.Player)
            || _stationInUse)
        {
            return;
        }

        _ui?.SetActive(true);
        _playerInRange = true;
        //Debug.Log(collision.gameObject);

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        //playerController.isNextToStation = true;
        playerController.currentStation = gameObject;
        playersByStation++;
        //OnPlayerNearStation?.Invoke(_assignedPlayer);

        //connectedPlayer = collision.gameObject
        // player in bounds = true
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(Tags.Player)
            || _stationInUse)
        {
            return;
        }

        playersByStation--;
        
        if (playersByStation == 0)
        {
            _ui?.SetActive(false);
            _playerInRange = false;
        }     

        collision.gameObject.GetComponent<PlayerController>().currentStation = null;
    }
    
    public void Initiate(GameObject player)
    {
        if (_stationInUse)
        {
            return;
        }

        _stationInUse = true;
        _assignedPlayer = player;
        _ui?.SetActive(false);
        Debug.Log("station in use");
    }

    public void Disengage()
    {
        if (!_stationInUse)
        {
            return;
        }

        _stationInUse = false;
        _assignedPlayer = null;
        _ui?.SetActive(true);

        // Reset art and stuff
        Debug.Log("STATION OUT OF USE");
    }
}

    /**
     * oNinteract(input action ctx)
      * - if player in bounds and ctx.started
      *     - initate station(connectedPlayer)
    void InteractWithStation()
    {
        //if station isPassive
            //check to see if isReady = true
            //if ready, give the play the item on complete, set isReady to false, and start the recharge timer
            //if not ready, show message saying item is not ready
        //if station is active
            //check to see if the player has the thing they need to activate the station (example: silk required to start loom)
                //if they have what they need
                    //start action timer coroutine
                    //do whatever player has to do to complete task (hold X, pres X a bunch, move joystick in given direction, etc)
                    //once task completed, give player item on complete
                //if they don't have what they need
                    //show message saying they are missing the item they need
    }
}
    */
