using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySlot : MonoBehaviour
{
    Image _icon;
    Image _slotFrame;

    private void Awake()
    {
        _icon = transform.GetChild(0).GetComponent<Image>();
        _slotFrame = GetComponent<Image>();
    }

    public void AssignIcon(Sprite icon)
    {
        _icon.sprite = icon;
        if(icon == null)
        {
            _icon.enabled = false;
        }
        else
        {
            _icon.enabled = true;
        }
    }

    public void Disable()
    {
        _slotFrame.enabled = false;
        _icon.sprite = null;
        _icon.enabled = false;
    }
    public void Enable()
    {
        _slotFrame.enabled = true;
    }
}
