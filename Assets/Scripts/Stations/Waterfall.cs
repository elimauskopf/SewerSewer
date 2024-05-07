using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Waterfall : MonoBehaviour
{

    public GameObject[] items;
    private float leftBound;
    private float rightBound;

    float repeatRate;
    [SerializeField]
    float repeatRateRangeMin;
    [SerializeField]
    float repeatRateRangeMax;


    private void Awake()
    {
        leftBound = transform.Find("LeftBound").transform.position.x;
        rightBound = transform.Find("RightBound").transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnItem()
    {
        float xSpawnLocation = Random.Range(leftBound, rightBound);
        Vector2 spawnLocation = new Vector3(xSpawnLocation, transform.position.y);
        int itemIndex = Random.Range(0, items.Length);

        GameObject item = Instantiate(items[itemIndex], spawnLocation, transform.rotation);
    }
}
