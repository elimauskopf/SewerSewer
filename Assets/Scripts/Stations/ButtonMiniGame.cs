using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMiniGame : MonoBehaviour
{

    public FishingStation fishingStation;

    GameObject _innerCircle;
    GameObject _outerCircle;

    public bool engaged;
    public bool isFishing;
    public Vector2 stickValue;
    private Vector2 oldStickValue;
    [SerializeField]
    float spinAngleDiff;
    private int _spinCounter;
    private float timeSinceLastSpin;
    [SerializeField]
    float spinCheckBuffer; 

    Color _originalColor;
    SpriteRenderer _innerCircleRenderer;

    float _t;
    float _startVal = 0;
    float _endVal = 50;
    float _lerpVal;
    [SerializeField]
    float perfectTimingamount;
    [SerializeField]
    float circleGrowAmount;

    private void Awake()
    {
        _innerCircle = transform.Find("InnerCircle").gameObject;
        _outerCircle = transform.Find("OuterCircle").gameObject;

        _innerCircleRenderer = _innerCircle.GetComponent<SpriteRenderer>();
        _originalColor = _innerCircleRenderer.color;
    }



    // Update is called once per frame
    void Update()
    {

        FillInnerCirle();

        IsSpinning();

    }



    void FillInnerCirle()
    {
        if (!engaged || _spinCounter < 1)
        {
            return;
        }
        /*_t += Time.deltaTime;
        _lerpVal = Mathf.Lerp(_startVal, _endVal, _t);*/
        _innerCircle.transform.localScale += new Vector3(circleGrowAmount, circleGrowAmount, circleGrowAmount) * Time.deltaTime;

        /*  if (_lerpVal >= perfectTimingamount
              )
          {
              _innerCircleRenderer.color = Color.green;
          }*/

        if (_innerCircle.transform.localScale.x > 6.6f)
        {
            fishingStation.CompleteTask();
            ResetInnerCircle();
        }

    }

    public bool PlayerPressedButton()
    {
        if (_lerpVal >= perfectTimingamount) // Great timing
        {
            ResetInnerCircle();
            return true;
        }

        return false;
    }


    void IsSpinning()
    {
        if (!engaged)
        {
            _spinCounter = 0;
            return;
        }

        if (!(timeSinceLastSpin + spinCheckBuffer < Time.time)) return;

        float angle = Vector2.Angle(oldStickValue, stickValue);

        if (angle > spinAngleDiff)
        {
            print("WE SPINNIN");
            _spinCounter++;
            timeSinceLastSpin = Time.time;
        }
        else
        {
            print("no spin");
            _spinCounter = 0;
        }


       

        oldStickValue = stickValue;
    }
    void ResetInnerCircle()
    {

        _innerCircle.transform.localScale = new Vector3(1, 1, 1);
        _t = 0;
        _lerpVal = 0;
        _innerCircleRenderer.color = _originalColor;
    }

    public void Hide()
    {
        _innerCircle.SetActive(false);
        _outerCircle.SetActive(false);
    }

    public void Show()
    {
        _innerCircle.SetActive(true);
        _outerCircle.SetActive(true);

        ResetInnerCircle();
    }

    public void EndInteraction()
    {
        engaged = false;
        Hide();
        ResetInnerCircle();
    }
}
