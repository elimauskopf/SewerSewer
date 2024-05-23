using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject bat;
    private GameManager gameManager;
    private float _minBoundY = -6f;
    private float maxBoundY = 3.5f;
    [Range(5, 50)]
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
            StartCoroutine(SpawnBat());
            _timer = 0;
            DecideNextSpawnTime();
        }
    }



    void DecideNextSpawnTime()
    {
        _nextSpawnTime = spawnFrequencyMin + Random.Range(0, spawnFrequencyVariance);
    }

    IEnumerator SpawnBat()
    {
        if (gameManager.levelCompleted) yield return null;

        float _ySpawnPoint = Random.Range(_minBoundY, maxBoundY);
        GameObject obj = Instantiate(bat, new Vector3(0, _ySpawnPoint , 0), bat.transform.rotation);
        GameObject obj2 = Instantiate(bat, new Vector3(0.5f, _ySpawnPoint -1f, 0), bat.transform.rotation);
        GameObject obj3 = Instantiate(bat, new Vector3(1, _ySpawnPoint + + 1f, 0), bat.transform.rotation);
        GameObject obj4 = Instantiate(bat, new Vector3(2f, _ySpawnPoint, 0), bat.transform.rotation);

        StartCoroutine(PlaySound(obj));


        yield return new WaitForSeconds(4);

        Destroy(obj);
        Destroy(obj2);
        Destroy(obj3);
        Destroy(obj4);
    }

    IEnumerator PlaySound(GameObject o)
    {
        yield return new WaitForSeconds(1f);

        o.GetComponent<AudioSource>().Play();


    }
}
