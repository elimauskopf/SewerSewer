using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{
    public ItemTypes itemType;
    public ColorTypes colorType;

    Rigidbody2D _rigidBody;

    bool _isOffScreen;
    float _offscreenTimer;
    float _timeUntilDestroy = 5f;

    float _rotationSpeed;
    float _minRotationSpeed = -80.0f;
    float _maxRotationSpeed = 80.0f;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rotationSpeed = Random.Range(_minRotationSpeed, _maxRotationSpeed);
        float _randomRotation = Random.Range(0, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, _randomRotation);
        _rigidBody.angularVelocity = _rotationSpeed;

    }

    private void Update()
    {
        if(_isOffScreen)
        {
            _offscreenTimer += Time.deltaTime;
        }

        if(_offscreenTimer > _timeUntilDestroy)
        {
            Destroy(gameObject);
        }


        //transform.Rotate(new Vector3(0, 0, Time.deltaTime * _rotationSpeed));
    }

    private void OnBecameInvisible()
    {
        _isOffScreen = true;
    }

    private void OnBecameVisible()
    {
        _isOffScreen = false;
        _offscreenTimer = 0;
    }
}
