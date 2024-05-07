using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public enum PlayerState { INSTATION, NONE, HIT };

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
    ItemTypes _itemOnStart;

    public ItemTypes? currentItem;
    public ColorTypes? currentColor;
    Transform _itemsParent;
    List<GameObject> _items = new List<GameObject>();

    public GameObject itemPrefab;

    //public bool isNextToStation;
    public GameObject currentStation;
    public bool workingStation;
    private bool _playerInHit;

    // Properties
    public PlayerState playerState { get; private set; }

    Vector2 _lookRight = new Vector2(-1, 1);
    Vector2 _lookLeft = new Vector2(1, 1);
    Vector2 _startScale;

    //public ItemObject CurrentItem { get { return _currentItem; } }

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
    [SerializeField]
    float hitStunTime;

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
        _startScale = transform.localScale;
    }

    void Start()
    {
        SetPlayerState(PlayerState.NONE);
        AssignItem(_itemOnStart, null);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alligator"))
        {
            if (!_playerInHit)
            {
                StartCoroutine(HitByAlligator());
            }
            
        }
    }
    public void InitiatePlayer(int playerNumber)
    {
        jacketFront.sprite = jacketFrontColors[playerNumber];
        jacketBack.sprite = jacketBackColors[playerNumber];
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
       

        //Made the following change so that the player doesn't slow down over time
        _movement = ctx.ReadValue<Vector2>();
        //MovePlayer(ctx.ReadValue<Vector2>());
    }


    public void OnInteract(InputAction.CallbackContext ctx)
    {
       

    }

    public void OnWork(InputAction.CallbackContext ctx)
    {
        if (ctx.started && currentStation)
        {
            if (!workingStation && currentStation)
            {
                if (currentStation.GetComponent<StationController>().Initiate(gameObject) == false)
                {
                    return;
                }
                workingStation = true;
                SetPlayerState(PlayerState.INSTATION);
                currentStation.GetComponent<StationController>().WorkStation();
            } else
            {
                currentStation.GetComponent<StationController>().WorkStation();
            }

        }
        
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started && _isGrounded 
            && playerState != PlayerState.HIT
           )
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
        workingStation = false;
        SetPlayerState(PlayerState.NONE);
        currentStation.GetComponent<StationController>().Disengage(gameObject);
     
        
    }

    void MovePlayer(Vector2 movementVector)
    {

        if (_playerInHit)
        {
            return;
        }

        if (movementVector.x > 0)
        {
            transform.localScale = _startScale * _lookRight;
        }
        else if (movementVector.x < 0)
        {
            transform.localScale = _startScale * _lookLeft;
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

    IEnumerator HitByAlligator()
    {

        _playerInHit = true;
        DropItem();
        _animator.SetTrigger(Tags.Hit);

        rb.velocity = new Vector2(0, jumpVelocity);

        yield return new WaitForSeconds(hitStunTime);

        

        _playerInHit = false;

    }

    public void DropItem()
    {
        currentItem = null;
        currentColor = null;
        AssignItem(currentItem, currentColor);
    }

    public void AssignItem(ItemTypes? newItem, ColorTypes? newColor)
    {
        currentItem = newItem;
        currentColor = newColor;
        foreach (GameObject item in _items)
        {
            if (newItem == null || !newItem.ToString().Equals(item.name))
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
                item.GetComponent<SpriteRenderer>().sprite = itemPrefab.GetComponent<ItemObject>().ChooseSprite(newItem, newColor);
            }
        }
    }
}
