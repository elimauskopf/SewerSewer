using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Waterfall : MonoBehaviour
{

    public GameObject[] items;
    public Sprite[] dyeSprites;
    private float leftBound;
    private float rightBound;
    public GameObject junk;

    //float repeatRate;
    [SerializeField]
    float repeatRateRangeMin;
    [SerializeField]
    float repeatRateRangeMax;


    private void Awake()
    {
        //junk = transform.Find("Junk").gameObject;
        leftBound = transform.Find("LeftBound").transform.position.x;
        rightBound = transform.Find("RightBound").transform.position.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnItem()
    {
        // Randomize spawn location
        float xSpawnLocation = Random.Range(leftBound, rightBound);
        Vector2 spawnLocation = new Vector3(xSpawnLocation, transform.position.y);

        // Randomize color
        Array colors = Enum.GetValues(typeof(ColorTypes));
        ColorTypes colorType = (ColorTypes)colors.GetValue(Random.Range(1, colors.Length));

        // Spawn item
        GameObject newJunk = Instantiate(junk, spawnLocation, transform.rotation);
        newJunk.GetComponent<Junk>().colorType = colorType;

        // Add sprite
        switch (colorType)
        {
                
            case ColorTypes.Red:
                newJunk.GetComponent<SpriteRenderer>().sprite = dyeSprites[0];
                break;
            case ColorTypes.Green:
                newJunk.GetComponent<SpriteRenderer>().sprite = dyeSprites[1];
                break;
            case ColorTypes.Yellow:
                newJunk.GetComponent<SpriteRenderer>().sprite = dyeSprites[2];
                break;
        }

        
    }

    IEnumerator SpawnCoroutine()
    {
        SpawnItem();

        yield return new WaitForSeconds(Random.Range(repeatRateRangeMin, repeatRateRangeMax));

        StartCoroutine(SpawnCoroutine());
    }

}
