using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.UI;

public class TimerMinigame : MonoBehaviour
{

    private GameObject parent;
    private GameObject slider;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private GameObject goalBar;

    public bool engaged;
    private bool isActive;

    [SerializeField]
    float sliderSpeed;
    [SerializeField]
    float sliderLowRange;
    [SerializeField]
    float sliderHighRange;

    private void Start()
    {
        parent = transform.Find("Parent").gameObject;
        slider = transform.Find("Parent/Slider").gameObject;
        startPoint = transform.Find("Parent/StartPoint").transform.position;
        endPoint = transform.Find("Parent/EndPoint").transform.position;
        goalBar = transform.Find("Parent/GoalBar").gameObject;

        print(startPoint);
        Hide();
    }

   

    // Update is called once per frame
    void Update()
    {
        if (!engaged) return;

        slider.transform.position +=  sliderSpeed * Time.deltaTime * Vector3.right;

        if (slider.transform.position.x >= endPoint.x )
        {
            slider.transform.position = startPoint;
        }

    }

    public void Hide()
    {
        parent.SetActive(false);
        isActive = false;
    }

    public void Show()
    {
       
        parent.SetActive(true);
        SetGoalBar();

        
        slider.transform.position = startPoint;
        isActive = true;
    }

    public void EndInteraction()
    {
        engaged = false;
        slider.transform.position = startPoint;
        Hide();
    }

    void SetGoalBar()
    {
        float xVal = Random.Range(startPoint.x + 0.1f, endPoint.x - 0.1f);
        goalBar.transform.position = new Vector2(xVal, goalBar.transform.position.y);
        print(goalBar.transform.position);
    }

    public bool WasButtonPressedOnTime()
    {
       

        if (slider.transform.position.x < (goalBar.transform.position.x + 0.5f) && slider.transform.position.x > (goalBar.transform.position.x - 0.5f))
        {

            slider.transform.position = startPoint;
            SetGoalBar();
            return true;
        }

        SetGoalBar();
        slider.transform.position = startPoint;
        return false;
    }


}
