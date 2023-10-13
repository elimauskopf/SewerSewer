using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarController : MonoBehaviour
{
    public Sprite[] chargeBarSprites = new Sprite[9];

    StationController _currentStation;
    SpriteRenderer _renderer;

    float _percentReloaded;
    bool _passiveStation;

    private void Awake()
    {
        _currentStation = transform.parent.GetComponent<StationController>();
        _renderer = GetComponent<SpriteRenderer>();
        _passiveStation = _currentStation.isPassive;
    }

    private void Update()
    {
        _percentReloaded = _currentStation.Timer / _currentStation.timeToComplete;
        AssignChargeBarSprite();
    }

    void AssignChargeBarSprite()
    {
        //active station renderer should be disabled if station not in use
        if(!_passiveStation && _percentReloaded == 0)
        {
            _renderer.enabled = false;
            return;
        }
        else if (_percentReloaded >= 1)
        {
            _renderer.enabled = false;
            return;
        }
        else
        {
            _renderer.enabled = true;
        }

        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(_percentReloaded * (chargeBarSprites.Length - 1)), 0, chargeBarSprites.Length - 1);

        _renderer.sprite = chargeBarSprites[spriteIndex];
    }

}
