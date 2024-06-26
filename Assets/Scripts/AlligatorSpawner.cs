using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlligatorSpawner : MonoBehaviour
{

    public GameObject alligator;
    private GameManager gameManager;
    private int _minBoundX = -17;
    private int maxBoundX = 18;
    private int _ySpawnPoint = -16;
    [Range(3, 50)]
    public int spawnFrequencyMin;
    [SerializeField]
    int spawnFrequencyVariance;

    float _timer;
    float _nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        _timer = 0;
        DecideNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.levelCompleted) return;
        _timer += Time.deltaTime;

        if (_timer > _nextSpawnTime)
        {
            //print(_nextSpawnTime);
            StartCoroutine(SpawnAlligator());
            _timer = 0;
            DecideNextSpawnTime();
        }
    }

    

    void DecideNextSpawnTime()
    {
        _nextSpawnTime = spawnFrequencyMin + Random.Range(0, spawnFrequencyVariance);
    }

    IEnumerator SpawnAlligator()
    {
        if (gameManager.levelCompleted) yield return null;

        float xSpawnPoint = Random.Range(_minBoundX, maxBoundX);
        GameObject obj = Instantiate(alligator, new Vector3(xSpawnPoint, _ySpawnPoint, 0), alligator.transform.rotation);

        StartCoroutine(PlaySound(obj));
        yield return new WaitForSeconds(2.17f);

        Destroy(obj);
    }
    
    IEnumerator PlaySound(GameObject o)
    {
        yield return new WaitForSeconds(1.45f);

        o.GetComponent<AudioSource>().Play();


    }

    
}
