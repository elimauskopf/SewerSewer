using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

  /*  private void FixedUpdate()
    {
        MovePlayer(_playerControls.Player.Movement.ReadValue<Vector2>()
    }*/

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        MovePlayer(ctx.ReadValue<Vector2>());   
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
