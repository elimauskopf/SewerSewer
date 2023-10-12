using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public Vector2[] spawnPoints;
    private PlayerInputManager playerInputManager;

    private string[] _playerNames = { "Player1", "Player2", "Player3", "Player4" };
    private int _playerNameIndex;


    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //Debug.Log(playerInput.gameObject.name);
        GameObject player = playerInput.gameObject;

        player.name = _playerNames[_playerNameIndex];
        playerInput.gameObject.transform.position = spawnPoints[_playerNameIndex];
        _playerNameIndex++;
    }
}
