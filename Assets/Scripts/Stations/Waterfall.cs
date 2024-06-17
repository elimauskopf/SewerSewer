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
    private float spawnYPos;
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
        spawnYPos = transform.Find("LeftBound").transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void SpawnItem()
    {
        // Randomize spawn location
        float xSpawnLocation = Random.Range(leftBound, rightBound);
        Vector2 spawnLocation = new Vector3(xSpawnLocation, spawnYPos);

        // Randomize color
        Array colors = Enum.GetValues(typeof(ColorTypes));
        ColorTypes colorType = (ColorTypes)colors.GetValue(Random.Range(1, colors.Length-1));

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

        yield return new WaitForSeconds(repeatRateRangeMin);

        StartCoroutine(SpawnCoroutine());
    }

}
