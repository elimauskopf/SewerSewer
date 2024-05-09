using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;

public class TimerMinigame : MonoBehaviour
{

    private Slider slider;
    public bool engaged;

    [SerializeField]
    float sliderSpeed;
    [SerializeField]
    float sliderLowRange;
    [SerializeField]
    float sliderHighRange;

    private void Awake()
    {
        slider = transform.Find("Canvas/Slider").GetComponent<Slider>();
        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!engaged) return;

        slider.value += sliderSpeed * Time.deltaTime;

        if (slider.value >= 1)
        {
            slider.value = 0;
        }

    }

    public void Hide()
    {
        slider.gameObject.SetActive(false);
       
    }

    public void Show()
    {
        slider.gameObject.SetActive(true);


        slider.value = 0;
    }

    public void EndInteraction()
    {
        engaged = false;     
        slider.value = 0;
        Hide();
    }

    public bool WasButtonPressedOnTime()
    {
        if(slider.value < sliderHighRange && slider.value > sliderLowRange)
        {

            slider.value = 0;
            return true;
        }

        slider.value = 0;
        return false;
    }


}
