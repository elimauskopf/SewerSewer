using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI Instance { get; private set; }

    public GameObject itemObjectPrefab;

    List<PlayerInventorySlot> _slots = new List<PlayerInventorySlot>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        foreach(Transform child in transform)
        {
            _slots.Add(child.GetComponent<PlayerInventorySlot>());
        }
    }
    private void Start()
    {
        foreach(PlayerInventorySlot slot in _slots)
        {
            slot.Disable();
        }
    }

    public void NewPlayerSpawned(int playerNumber)
    {
        _slots[playerNumber].Enable();
    }

    public void AssingPlayerItem(int playerIndex, Sprite icon)
    {
        _slots[playerIndex].AssignIcon(icon);
    }
}
