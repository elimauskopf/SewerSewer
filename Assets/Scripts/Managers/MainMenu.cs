using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private enum State { REGIONSELECT, LEVELSELECT}
    public GameObject boat;
    public GameObject start;
    public GameObject exit;
    public GameObject regionButtons;
    public GameObject regionOneLevels;
    public GameObject regionTwoLevels;
    public GameObject regionThreeLevels;
    public GameObject backButton;

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Back()
    {
        switch (state)
        {
            case State.REGIONSELECT:
                HideLevelMenu();
                break;
            case State.LEVELSELECT:
                HideLevelSelectMenus();
                break;
        }
    }

    public void OpenLevelMenu()
    {
        boat.SetActive(false);
        start.SetActive(false);
        exit.SetActive(false);

        OpenRegionSelectMenu();

        backButton.SetActive(true);

    }

    public void HideLevelMenu()
    {
        boat.SetActive(true);
        start.SetActive(true);
        exit.SetActive(true);

        regionButtons.SetActive(false);
        backButton.SetActive(false);
    }

    public void OpenRegionSelectMenu()
    {
        state = State.REGIONSELECT;
        regionButtons.SetActive(true);
    }

    public void HideRegionSelectMenu()
    {
        regionButtons.SetActive(false);
    }

    public void OpenRegionOneMenu()
    {
        HideRegionSelectMenu();
        regionOneLevels.SetActive(true);
        state = State.LEVELSELECT;
    }

    public void OpenRegionTwoMenu()
    {
        HideRegionSelectMenu();
        regionTwoLevels.SetActive(true);
        state = State.LEVELSELECT;
    }

    public void OpenRegionThreeMenu()
    {
        HideRegionSelectMenu();
        regionThreeLevels.SetActive(true);
        state = State.LEVELSELECT;
    }

    public void HideLevelSelectMenus()
    {
        regionOneLevels.SetActive(false);
        regionTwoLevels.SetActive(false);
        regionThreeLevels.SetActive(false);

        OpenRegionSelectMenu();
    }
}
