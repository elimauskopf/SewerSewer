using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    [SerializeField]
    protected Sprite _blueCircle, _redCircle;
    protected Animator _animator;
    protected GameObject _uiButton;
    protected SpriteRenderer _uiRenderer;
    protected GameObject _chargeBarObject;
    protected GameObject _iconObject;
    protected SpriteRenderer _iconRenderer;
    protected ChargeBarController _chargeBarController;

    public delegate void StationAction(GameObject player);
    public static event StationAction OnPlayerNearStation;
    public static event StationAction OnPlayerExitStation;

    // Player currently controlling station
    protected GameObject _assignedPlayer;
    protected int playersByStation;

    //does the station recharge on its own or require player participation to charge
    public bool isPassive;

    //how long does it take the station to charge up
    public float timeToComplete;

    //what does the station give the player when the task is complete (example: spiders give silk)
    //(data type might change from GameObject to something custom)
    public ItemObject itemOnCompletion;

    //what does the station require from the player to activate (example: loom requires spider silk)
    public ItemObject itemRequiredToStart;

    protected bool _playerInRange;
    protected float _timer;
    protected bool _isReadyToHarvest;
    protected bool _isAbleToCharge;
    public bool stationInUse;

    protected Color _translucent = new Color(0.7f, 0.7f, 0.7f, 1);

    public float Timer { get { return _timer; } }
    public bool IsAbleToCharge { get { return _isAbleToCharge; } }

    // Connected player

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _uiButton = transform.Find(Tags.UI)?.gameObject;
        _uiRenderer = _uiButton?.GetComponent<SpriteRenderer>();

        _chargeBarObject = transform.Find(Tags.ChargeBar)?.gameObject;
        _chargeBarController = _chargeBarObject?.GetComponent<ChargeBarController>();

        _iconObject = transform.Find(Tags.Icon)?.gameObject;
        _iconRenderer = _iconObject?.GetComponent<SpriteRenderer>();
        if(itemRequiredToStart != null && _iconRenderer)
        {
            _iconRenderer.sprite = itemRequiredToStart.icon;
        }

        _uiButton?.SetActive(false);
        _iconObject?.SetActive(false);
    }

    protected virtual void Update()
    {
        if (_timer < timeToComplete && isPassive && _isAbleToCharge)
        {
            _timer += Time.deltaTime;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag(Tags.Player)
            )
        {
            return;
        }

        AssignUI();
        _playerInRange = true;

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if(playerController == null)
        {
            return;
        }

        if(itemRequiredToStart == null)
        {
            //lmao
        }
        else if(playerController.CurrentItem == null)
        {
            _iconRenderer.color = _translucent;
        }
        else if(playerController.CurrentItem.type.Equals(itemRequiredToStart.type))
        {
            _iconRenderer.color = Color.white;
        }
        else
        {
            _iconRenderer.color = _translucent;
        }

        playerController.currentStation = gameObject;
        playersByStation++;

    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(Tags.Player))
        {
            return;
        }


        if (playersByStation != 0)
        {
            playersByStation--;
        }
     
        
        if (playersByStation == 0)
        {
            _playerInRange = false;
            DisableUI();
        }

        if(collision.gameObject.GetComponent<PlayerController>().currentStation && 
            collision.gameObject.GetComponent<PlayerController>().currentStation.name.Equals(gameObject.name))
        {
            collision.gameObject.GetComponent<PlayerController>().LeaveStation();
            
        }
        else
        {
            collision.gameObject.GetComponent<PlayerController>().currentStation = null;

        }

        _iconObject?.SetActive(false);
    }
    
    public virtual bool Initiate(GameObject player)
    {
        if (stationInUse)
        {
            Debug.Log("Station is in use");
            return false;
        }

        PlayerController currentPlayer = player.GetComponent<PlayerController>();

        if (isPassive)
        {
            if(_timer == 0)//station is not active, waiting for input
            {
                if(HandlePlayerItem(currentPlayer))
                {
                    _isAbleToCharge=true;
                    _chargeBarController?.StartChargeBar();
                }
            }
            else if(_timer >= timeToComplete)//station is ready
            {
                
                _assignedPlayer = player;
                CompleteTask();
            }
            return false;
        }
        else //if station is active (not passive)
        {
            if (!_isAbleToCharge && !HandlePlayerItem(currentPlayer))
            {
                return false;
            }

            _assignedPlayer = player;
            stationInUse = true;
            _chargeBarController?.StartChargeBar();
            _animator.SetBool(Tags.Moving, true);

            if (!_isAbleToCharge)
            {
                _isAbleToCharge = true;
                Debug.Log("station in use");
            }
            AssignUI();
            return true;
        }
    }

    protected bool HandlePlayerItem(PlayerController currentPlayer)
    {
        if (currentPlayer == null)
        {
            return false;
        }

        if (itemRequiredToStart == null)
        {
            currentPlayer.DropItem();
            return true;
        }
        else if (currentPlayer.CurrentItem == null)
        {
            Debug.Log("Player isn't carrying anything");
            return false;
        }
        else if (currentPlayer.CurrentItem.type.Equals(itemRequiredToStart.type))
        {
            currentPlayer.DropItem();
            return true;
            
        }

        return false;
    }
    public void Disengage()
    {
        if (!stationInUse)
        {
            return;
        }

        _animator.SetBool(Tags.Moving, false);
        stationInUse = false;
        _assignedPlayer = null;
        _uiButton?.SetActive(true);
        _chargeBarController?.HideChargeBar();
    }

    public void WorkStation()
    {
        _chargeBarController?.AddCharge();

        if (_chargeBarController && _chargeBarController.percentReloaded >=1 )
        {
            // Player finished station
            CompleteTask();
        }
    }

    protected virtual void CompleteTask()
    {
        if(isPassive)
        {
            _timer = 0;
            _chargeBarController.HideChargeBar();
        }
        else
        {
            _animator.SetBool(Tags.Moving, false);
        }
        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        Debug.Log("Completing task, giving player " + itemOnCompletion);
         _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion);
        _assignedPlayer.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }

    protected virtual void AssignUI()
    {
        _uiButton?.SetActive(true);
        _iconObject?.SetActive(true);
        if(stationInUse || _isAbleToCharge)
        {
            _uiRenderer.sprite = _redCircle;
            _iconObject?.SetActive(false);
        }
        else
        {
            _uiRenderer.sprite = _blueCircle;
            _iconObject?.SetActive(true);
        }
    }
    protected virtual void DisableUI()
    {
        if (stationInUse || _isAbleToCharge)
        {
            //dont disable uiButton
        }
        else
        {
            _uiButton?.SetActive(false);
        }
        _iconObject?.SetActive(false);
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
