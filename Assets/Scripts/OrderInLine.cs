using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLine : MonoBehaviour
{
    Vector3 oldPosition;
    Vector3 newPosition;

    float timer;

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
        while(transform.position != newPosition)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, timer / LineManager.Instance.timeForOrderToMoveForward);
            yield return null;
        }
    }

    public void CompleteOrder()
    {
        Destroy(gameObject);
    }
}
