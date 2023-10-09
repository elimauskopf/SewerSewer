using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    //does the station recharge on its own or require player participation to charge
    public bool isPassive;

    //how long does it take the station to charge up
    public float timeToComplete;

    //what does the station give the player when the task is complete (example: spiders give silk)
    //(data type might change from GameObject to something custom)
    public GameObject itemOnCompletion;

    //what does the station require from the player to activate (example: loom requires spider silk)
    public GameObject itemRequiredToStart;

    GameObject _ui;
    bool _playerInRange;
    bool _timer;
    bool _isReady;

    private void Awake()
    {
        _ui = transform.Find(Tags.UI)?.gameObject;
        _ui?.SetActive(false);
    }

    private void Update()
    {
        //if player presses action button
        //call InteractWithStation()
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.transform.CompareTag(Tags.Player))
        {
            return;
        }

        _ui?.SetActive(true);
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag(Tags.Player))
        {
            return;
        }

        _ui?.SetActive(false);
        _playerInRange = false;
    }

    void InteractWithStation()
    {
        //if station isPassive
            //check to see if isReady = true
            //if ready, give the play the item on complete, set isReady to false, and start the recharge timer
            //if not ready, show message saying item is not ready
        //if station is active
            //check to see if the player has the thing they need to activate the station (example: silk required to start loom)
                //if they have what they need
                    //start action timer coroutine
                    //do whatever player has to do to complete task (hold X, pres X a bunch, move joystick in given direction, etc)
                    //once task completed, give player item on complete
                //if they don't have what they need
                    //show message saying they are missing the item they need
    }
}
