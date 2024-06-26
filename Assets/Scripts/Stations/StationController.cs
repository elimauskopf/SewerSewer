using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{

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
    public ItemTypes itemOnCompletion;

    //what does the station require from the player to activate (example: loom requires spider silk)
    public List<ItemTypes> itemsRequiredToStart = new List<ItemTypes>();

    //what icon should we show the player based on what item the station requires
    public Sprite itemRequiredIcon;

    //does the station have an item inside it currently
    protected ItemTypes _currentItemType;
    protected ColorTypes? _currentColor;

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
        if (itemsRequiredToStart.Count > 0 && _iconRenderer)
        {
            _iconRenderer.sprite = itemRequiredIcon;
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

        _playerInRange = true;

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }

        foreach (ItemTypes? item in itemsRequiredToStart)
        {
            if (item == null) //item not assigned (probably a mistake on our end)
            {
                //lmao
            }
            else if (playerController.currentItem.Equals(ItemTypes.None))//if the player doesn't have anything and the station requires something
            {
                _iconRenderer.color = _translucent;
            }
            else if (playerController.currentItem.Equals(item))//if the player has what is required
            {
                _iconRenderer.color = Color.white;
                break;
            }
            else//if the player has the wrong item
            {
                _iconRenderer.color = _translucent;
            }
        }


        playerController.currentStation = gameObject;
        playersByStation++;

        AssignUI();
       

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

        if (collision.gameObject.GetComponent<PlayerController>().currentStation &&
            collision.gameObject.GetComponent<PlayerController>().currentStation.name.Equals(gameObject.name))
        {
            collision.gameObject.GetComponent<PlayerController>().LeaveStation();
        }

        collision.gameObject.GetComponent<PlayerController>().currentStation = null;

        _iconObject?.SetActive(false);
    }

    //used when player interacts with station
    public virtual bool Initiate(GameObject player)
    {
        if (stationInUse)
        {
            Debug.Log("Station is in use");
            return false;
        }


        PlayerController currentPlayer = player.GetComponent<PlayerController>();
        print(currentPlayer.currentStation);

        if (!currentPlayer.currentStation) return false;


        if (isPassive)
        {
            if (_timer == 0)//station is not active, waiting for input
            {
                if (HandlePlayerItem(currentPlayer))
                {
                    _isAbleToCharge = true;
                    _chargeBarController?.StartChargeBar();
                }
            }
            else if (_timer >= timeToComplete)//station is ready
            {

                if (HandlePlayerItem(currentPlayer)) // player has fish and spider is done
                {
                    _assignedPlayer = player;
                    CompleteTask();
                    _isAbleToCharge = true;
                    _chargeBarController?.StartChargeBar();

                }
                else
                {
                    _assignedPlayer = player;
                    CompleteTask();
                }
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

    //check to see if the player has the required item to use the station
    protected virtual bool HandlePlayerItem(PlayerController currentPlayer)
    {
        if (currentPlayer == null)
        {
            return false;
        }

        //if the station doesn't require anything, 
        if (itemsRequiredToStart.Count == 0)
        {
            return true;
        }
        else if (currentPlayer.currentItem.Equals(ItemTypes.None))//if the player doesn't have anything 
        {
            Debug.Log("Player isn't carrying anything");
            return false;
        }

        foreach (ItemTypes item in itemsRequiredToStart)
        {
            if (currentPlayer.currentItem.Equals(item))//if the player has a required item
            {
                _currentItemType = currentPlayer.currentItem;
                _currentColor = currentPlayer.currentColor;
                currentPlayer.DropItem();
                return true;
            }
        }

        return false;
    }
    public virtual void Disengage(GameObject player)
    {
        if (!stationInUse)
        {
            return;
        }

        if (_assignedPlayer == player)
        {
            print("leave");
            _assignedPlayer = null;
            stationInUse = false;


        }

        AssignUI();
    }

    public virtual void WorkStation()
    {

        _chargeBarController?.AddCharge();


        if (_chargeBarController && _chargeBarController.percentReloaded >= 1)
        {
            // Player finished station
            CompleteTask();
        }
    }

    public virtual void CompleteTask()
    {
        if (isPassive)
        {
            _timer = 0;
            _chargeBarController.HideChargeBar();
        }
        else
        {
            _animator.SetBool(Tags.Moving, false);
        }

        // If dress add to order
        if (itemOnCompletion.Equals(ItemTypes.Dress))
        {
            _assignedPlayer.GetComponent<PlayerController>().currentOrder = new Order(new ItemObject(ItemTypes.Dress, _currentColor), new ItemObject(ItemTypes.None, ColorTypes.None));
        }

        _chargeBarController.ResetChargeBar();
        _isAbleToCharge = false;
        _currentItemType = ItemTypes.None;
        _assignedPlayer?.GetComponent<PlayerController>().AssignItem(itemOnCompletion, _currentColor);
        _assignedPlayer?.GetComponent<PlayerController>().LeaveStation();
        AssignUI();
    }

    protected virtual void AssignUI()
    {
        _uiButton?.SetActive(true);
        _iconObject?.SetActive(true);

        if (stationInUse || _isAbleToCharge)
        {
            //_uiRenderer.sprite = _redCircle;
            _iconObject?.SetActive(false);

        }
        else if (playersByStation == 0)
        {
            _uiButton?.SetActive(false);
            _iconObject?.SetActive(false);
            _chargeBarController?.HideChargeBar();


        }
        else
        {
            //_uiRenderer.sprite = _blueCircle;
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
