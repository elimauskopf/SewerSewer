using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLine : MonoBehaviour
{
    Animator _animator;

    Vector3 oldPosition;
    Vector3 newPosition;

    float timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
