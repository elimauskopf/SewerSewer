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


    private void Awake()
    {

       // _playerControls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

   /* private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    } */

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
}
