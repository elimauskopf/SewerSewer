using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] List<Sprite> shoeColors;
    [SerializeField] List<Sprite> dressColors;
    [SerializeField] List<Sprite> ribbonColors;

    Animator _animator;
    SpriteRenderer _leftShoe, _rightShoe;

    Vector3 oldPosition;
    Vector3 newPosition;

    // Order vars
    public Order order;
    private SpriteRenderer dress;
    private SpriteRenderer ribbon;

    float timer;

    private void Awake()
    {
        order = new Order(new ItemObject(ItemTypes.Dress, ColorTypes.White));

        _animator = GetComponent<Animator>();
        _leftShoe = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _rightShoe = transform.GetChild(1).GetComponent<SpriteRenderer>();

        int color = Random.Range(0, shoeColors.Count - 1);
        _leftShoe.sprite = shoeColors[color];
        _rightShoe.sprite = shoeColors[color];

        dress = transform.Find("Order/Dress").GetComponent<SpriteRenderer>();
        ribbon = transform.Find("Order/Ribbon").GetComponent<SpriteRenderer>();
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
        while (transform.position != newPosition)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, timer / LineManager.Instance.timeForOrderToMoveForward);
            yield return null;
        }
        _animator.SetBool(Tags.Moving, false);
    }

    public void PopulateOrder(Order newOrder)
    {
        order = newOrder;

        //print(order.dress.ColorType);

        switch (order.dress.ColorType)
        {
            case ColorTypes.White:
                dress.sprite = dressColors[0];
                break;
            case ColorTypes.Red:
                dress.sprite = dressColors[1];
                break;
            case ColorTypes.Green:
                dress.sprite = dressColors[2];
                break;
            case ColorTypes.Yellow:
                dress.sprite = dressColors[3];
                break;
        }

        if (!GameManager.Instance.isRegionThree)
        {
            ribbon.enabled = false;
            return;
        }

        switch (order.ribbon.ColorType)
        {
            case ColorTypes.White:
                ribbon.sprite = ribbonColors[0];
                break;
            case ColorTypes.Red:
                ribbon.sprite = ribbonColors[1];
                break;
            case ColorTypes.Green:
                ribbon.sprite = ribbonColors[2];
                break;
            case ColorTypes.Yellow:
                ribbon.sprite = ribbonColors[3];
                break;
        }


    }
    public void CompleteOrder()
    {
        Destroy(gameObject);
    }
}
