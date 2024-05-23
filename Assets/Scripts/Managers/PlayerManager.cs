using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Vector2[] spawnPoints;
    private PlayerInputManager playerInputManager;

    private string[] _playerNames = { "Player1", "Player2", "Player3", "Player4" };
    private int _playerNameIndex;

    public int PlayerIndex { get { return _playerNameIndex; } }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //Debug.Log(playerInput.gameObject.name);
        GameObject player = playerInput.gameObject;

        player.name = _playerNames[_playerNameIndex];
        player.GetComponent<PlayerController>()?.InitiatePlayer(_playerNameIndex);
        PlayerInventoryUI.Instance.NewPlayerSpawned(_playerNameIndex);
        playerInput.gameObject.transform.position = spawnPoints[_playerNameIndex];
        _playerNameIndex++;
    }
}
