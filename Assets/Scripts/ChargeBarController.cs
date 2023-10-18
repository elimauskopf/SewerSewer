using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBarController : MonoBehaviour
{
    public Sprite[] chargeBarSprites = new Sprite[9];

    StationController _currentStation;
    SpriteRenderer _renderer;

    public float percentReloaded;
    [SerializeField]
    float _barChargeIncrement;
    [SerializeField]
    //bool _passiveStation;

    private void Awake()
    {
        _currentStation = transform.parent.GetComponent<StationController>();
        _renderer = GetComponent<SpriteRenderer>();
        //_passiveStation = _currentStation.isPassive;
    }

    private void Start()
    {
        AssignChargeBarSprite();
    }

    private void Update()
    {
        //percentReloaded = _currentStation.Timer / _currentStation.timeToComplete;
        if (_currentStation.stationInUse || _currentStation.IsAbleToCharge)
        {
            UpdateChargeBar();
        }
        
    }

    void AssignChargeBarSprite()
    {
        //active station renderer should be disabled if station not in use
        if(!_currentStation.isPassive && percentReloaded == 0)
        {
            _renderer.enabled = false;
            return;
        }
        else if (percentReloaded >= 1)
        {
            _renderer.enabled = false;
            return;
        }
        else
        {
            _renderer.enabled = true;
        }
        UpdateChargeBar();
    }

    public void AddCharge()
    {
        if (percentReloaded + _barChargeIncrement > 1)
        {
            percentReloaded = 1;
        } else
        {
            percentReloaded += _barChargeIncrement;
        }
    } 

    public void UpdateChargeBar()
    {
        if (_currentStation.isPassive)
        {
            percentReloaded = _currentStation.Timer / _currentStation.timeToComplete;
        }
        int spriteIndex = Mathf.Clamp(Mathf.FloorToInt(percentReloaded * (chargeBarSprites.Length - 1)), 0, chargeBarSprites.Length - 1);

        _renderer.sprite = chargeBarSprites[spriteIndex];
    }

    public void StartChargeBar()
    {
        _renderer.enabled = true;
    }

    public void HideChargeBar()
    {
        _renderer.enabled = false;
    }

    public void ResetChargeBar()
    {
        percentReloaded = 0;
        HideChargeBar();
    }
}
