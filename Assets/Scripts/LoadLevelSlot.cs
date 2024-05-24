using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelSlot : MonoBehaviour
{
    public Scenes sceneToLoad;

    Transform _stars;

    float _timeRemaining;

    private void Awake()
    {
        _stars = transform.Find("Stars");
        LoadStars();
    }
    public void OnClick()
    {
        SceneNavigator.Instance.LoadScene(sceneToLoad.ToString());
    }

    void LoadStars()
    {
        foreach (Transform star in _stars)
        {
            star.gameObject.SetActive(false);
        }

        _timeRemaining = ES3.Load(Tags.TimeRemaining + sceneToLoad.ToString(), -1f);
        switch(StarCount.NumberOfStars(_timeRemaining))
        {
            case 1:
                _stars.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                _stars.GetChild(0).gameObject.SetActive(true);
                _stars.GetChild(1).gameObject.SetActive(true);
                break;
            case 3:
                _stars.GetChild(0).gameObject.SetActive(true);
                _stars.GetChild(1).gameObject.SetActive(true);
                _stars.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
