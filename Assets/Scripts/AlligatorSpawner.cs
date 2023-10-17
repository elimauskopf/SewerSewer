using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlligatorSpawner : MonoBehaviour
{

    public GameObject alligator;
    private int _minBoundX = -17;
    private int maxBoundX = 18;
    private int _ySpawnPoint = -16;
    [Range(5, 50)]
    public int spawnFrequencyMin;
    [SerializeField]
    int spawnFrequencyVariance;

    float _timer;
    float _nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
        DecideNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _nextSpawnTime)
        {
            print(_nextSpawnTime);
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
        float xSpawnPoint = Random.Range(_minBoundX, maxBoundX);
        GameObject obj = Instantiate(alligator, new Vector3(xSpawnPoint, _ySpawnPoint, 0), alligator.transform.rotation);

        yield return new WaitForSeconds(2.17f);

        Destroy(obj);
    }

    
}
