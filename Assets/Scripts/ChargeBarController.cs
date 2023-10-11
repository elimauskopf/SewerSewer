using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarController : MonoBehaviour
{
    public Sprite[] chargeBarSprites = new Sprite[9];

    StationController _currentStation;
    SpriteRenderer _renderer;

    float _percentReloaded;

    private void Awake()
    {
        _currentStation = transform.parent.GetComponent<StationController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _percentReloaded = _currentStation.Timer / _currentStation.timeToComplete;
        AssignChargeBarSprite();
    }

    void AssignChargeBarSprite()
    {
        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(_percentReloaded * (chargeBarSprites.Length - 1)), 0, chargeBarSprites.Length - 1);

        _renderer.sprite = chargeBarSprites[spriteIndex];
    }

}
