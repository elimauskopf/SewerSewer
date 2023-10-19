using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMiniGame : MonoBehaviour
{

    GameObject _innerCircle;
    GameObject _outerCircle;

    public bool engaged;

    Color _originalColor;
    SpriteRenderer _innerCircleRenderer;

    float _t;
    float _startVal = 1;
    float _endVal = 7;
    float _lerpVal;
    [SerializeField]
    float perfectTimingamount;

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
        if (engaged)
        {
            FillInnerCirle();
        }
        
    }

    void FillInnerCirle()
    {
        _t += Time.deltaTime;
        _lerpVal = Mathf.Lerp(_startVal, _endVal, _t);
        _innerCircle.transform.localScale = new Vector3(_lerpVal, _lerpVal, _lerpVal);

      /*  if (_lerpVal >= perfectTimingamount
            )
        {
            _innerCircleRenderer.color = Color.green;
        }*/

        if (Mathf.Approximately(_lerpVal, _endVal))
        {
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

    void ResetInnerCircle()
    {
        
        _innerCircle.transform.localScale = new Vector3(1, 1, 1);
        _t = 0;
        _startVal = 0;
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
