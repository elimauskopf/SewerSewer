using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum PlayerState { INSTATION, NONE };

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer jacketFront;
    public SpriteRenderer jacketBack;

    [SerializeField]
    List<Sprite> jacketFrontColors;
    [SerializeField]
    List<Sprite> jacketBackColors;

    Rigidbody2D rb;
    Animator _animator;
    Transform rayCastTransform;
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

    Vector2 _lookRight = new Vector2(-1, 1);
    Vector2 _lookLeft = new Vector2(1, 1);

    public ItemObject CurrentItem { get { return _currentItem; } }

    // Movement vars
    [SerializeField]
    bool _isGrounded;
    RaycastHit2D groundCheckRay;
    [SerializeField]
    LayerMask groundCheckLayers;
    [SerializeField]
    float circleCastRadius;
    [SerializeField]
    float jumpVelocity;
    [SerializeField]
    float _airSpeedModifer;

    //added by jonah to test something out
    Vector2 _movement;


    private void Awake()
    {

        // _playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _itemsParent = transform.GetChild(0).Find(Tags.Items);
        foreach (Transform child in _itemsParent)
        {
            _items.Add(child.gameObject);
        }

        rayCastTransform = transform.Find("BottomPlayer");
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
        IsGroundedCheck();

        if (rb.velocity.magnitude <= 0.05)
        {
            _animator.SetBool(Tags.Moving, false);
        }
    }

    public void SetPlayerState(PlayerState newState)
    {
        playerState = newState;
    }

    private void FixedUpdate()
    {
        MovePlayer(_movement);
    }

    public void InitiatePlayer(int playerNumber)
    {
        jacketFront.sprite = jacketFrontColors[playerNumber];
        jacketBack.sprite = jacketBackColors[playerNumber];
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (playerState == PlayerState.INSTATION)
        {
            return;
        }

        //Made the following change so that the player doesn't slow down over time
        _movement = ctx.ReadValue<Vector2>();
        //MovePlayer(ctx.ReadValue<Vector2>());
    }


    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!workingStation && currentStation)
            {
                if (currentStation.GetComponent<StationController>().Initiate(gameObject) == false)
                {
                    return;
                }
                workingStation = true;
                SetPlayerState(PlayerState.INSTATION);
            }
            else if (workingStation)
            {
                LeaveStation();
            }
        }

    }

    public void OnWork(InputAction.CallbackContext ctx)
    {
        if (ctx.started && workingStation)
        {
            currentStation.GetComponent<StationController>().WorkStation();

        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && _isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    void IsGroundedCheck()
    {
        groundCheckRay = Physics2D.CircleCast(rayCastTransform.position, circleCastRadius, -Vector2.up, 0, groundCheckLayers);

        //print(groundCheckRay.collider.gameObject.layer);
        if (groundCheckRay.collider && groundCheckRay.collider.gameObject.layer == 6)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    public void LeaveStation()
    {
        currentStation.GetComponent<StationController>().Disengage();
        workingStation = false;
        SetPlayerState(PlayerState.NONE);
    }

    void MovePlayer(Vector2 movementVector)
    {

        if (movementVector.x > 0)
        {
            transform.localScale = _lookRight;
        }
        else if (movementVector.x < 0)
        {
            transform.localScale = _lookLeft;
        }

        Vector2 newVelocity;
        if (_isGrounded)
        {
            newVelocity = new Vector2
            {
                x = movementVector.x * _speed,
                y = rb.velocity.y
            };
        } else 
        {
            newVelocity = new Vector2
            {
                x = movementVector.x * _speed * _airSpeedModifer,
                y = rb.velocity.y
            };
        }
        

        rb.velocity = newVelocity;



        _animator.SetBool(Tags.Moving, true);


    }

    

    public void DropItem()
    {
        _currentItem = null;
        AssignItem(null);
    }

    public void AssignItem(ItemObject newItem)
    {
        _currentItem = newItem;
        foreach (GameObject item in _items)
        {
            if (newItem == null || !newItem.type.ToString().Equals(item.name))
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }

    /*void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rayCastTransform.position, circleCastRadius);
    }*/
}
