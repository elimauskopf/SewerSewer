using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] List<Sprite> shoeColors;

    Animator _animator;
    SpriteRenderer _leftShoe, _rightShoe;

    Vector3 oldPosition;
    Vector3 newPosition;

    float timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _leftShoe = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _rightShoe = transform.GetChild(1).GetComponent<SpriteRenderer>();

        int color = Random.Range(0, shoeColors.Count - 1);
        _leftShoe.sprite = shoeColors[color];
        _rightShoe.sprite = shoeColors[color];
    }

    public void MoveTowards(Vector3 position)
    {
        StopAllCoroutines();
        newPosition = position;
        oldPosition = transform.position;
        StartCoroutine(MovingTowardsPoint());
    }

    IEnumerator MovingTowardsPoint()
    {
        timer = 0;
        _animator.SetBool(Tags.Moving, true);
        while(transform.position != newPosition)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, timer / LineManager.Instance.timeForOrderToMoveForward);
            yield return null;
        }
        _animator.SetBool(Tags.Moving, false);
    }

    public void CompleteOrder()
    {
        Destroy(gameObject);
    }
}
