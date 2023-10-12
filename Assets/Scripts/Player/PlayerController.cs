using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum PlayerState { INSTATION, NONE};

public class PlayerController : MonoBehaviour
{
   
    Rigidbody2D rb;
    Animator _animator;
    //private PlayerControls _playerControls;

    [SerializeField]
    float _speed;

    [SerializeField]//just used for testing, player will not start with an item
    ItemObject _itemOnStart;

    ItemObject _currentItem;
    Transform _itemsParent;
    List<GameObject> _items = new List<GameObject>();

    //public bool isNextToStation;
    public GameObject currentStation;
    public bool workingStation;

    // Properties
    public PlayerState playerState { get; private set; }

    private void Awake()
    {

       // _playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _itemsParent = transform.GetChild(0).Find(Tags.Items);
        foreach(Transform child in _itemsParent)
        {
            _items.Add(child.gameObject);
        }
    }

    void Start()
    {
        SetPlayerState(PlayerState.NONE);
        AssignItem(_itemOnStart);

    }

   /* private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    } */


    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude <= 0.05)
        {
            _animator.SetBool(Tags.Moving, false);
        }
    }

    public void SetPlayerState(PlayerState newState)
    {
        playerState = newState;
    }

    /*  private void FixedUpdate()
      {
          MovePlayer(_playerControls.Player.Movement.ReadValue<Vector2>()
      }*/

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (playerState == PlayerState.INSTATION)
        {
            return;
        }

        MovePlayer(ctx.ReadValue<Vector2>());   
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!workingStation && currentStation)
        {
            currentStation.GetComponent<StationController>().Initiate(gameObject);
            workingStation = true;
            SetPlayerState(PlayerState.INSTATION);
        } else if(workingStation)
        {
            currentStation.GetComponent<StationController>().Disengage();
            workingStation = false;
            SetPlayerState(PlayerState.NONE);

        }
    }

    void MovePlayer(Vector2 movementVector)
    {
        rb.velocity = movementVector * _speed;
        _animator.SetBool(Tags.Moving, true);
    }

    public void DropItem()
    {
        _currentItem = null;
    }

    public void AssignItem(ItemObject newItem)
    {
        foreach(GameObject item in _items)
        {
            if(newItem.type.ToString().Equals(item.name))
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    
}
