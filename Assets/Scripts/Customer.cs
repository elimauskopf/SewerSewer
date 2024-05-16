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
    private SpriteRenderer dressComponent;
    private SpriteRenderer ribbonComponent;
    private SpriteRenderer finalDress;
    private SpriteRenderer finalRibbon;
    private

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

        dressComponent = transform.Find("Order/Dress").GetComponent<SpriteRenderer>();
        ribbonComponent = transform.Find("Order/Ribbon").GetComponent<SpriteRenderer>();
        finalDress = transform.Find("Order/MainDress").GetComponent<SpriteRenderer>();
        finalRibbon = transform.Find("Order/MainRibbon").GetComponent<SpriteRenderer>();
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

        switch (order.dress.ColorType)
        {
            case ColorTypes.White:
                finalDress.sprite = dressColors[0];
                break;
            case ColorTypes.Red:
                finalDress.sprite = dressColors[1];
                break;
            case ColorTypes.Green:
                finalDress.sprite = dressColors[2];
                break;
            case ColorTypes.Yellow:
                finalDress.sprite = dressColors[3];
                break;
        }

        if (!GameManager.Instance.isRegionThree)
        {
            ribbonComponent.enabled = false;
            finalRibbon.enabled = false;
            dressComponent.enabled = false;
            return;
        }

        switch (order.ribbon.ColorType)
        {
            case ColorTypes.White:
                finalRibbon.sprite = ribbonColors[0];
                break;
            case ColorTypes.Red:
                finalRibbon.sprite = ribbonColors[1];
                break;
            case ColorTypes.Green:
                finalRibbon.sprite = ribbonColors[2];
                break;
            case ColorTypes.Yellow:
                finalRibbon.sprite = ribbonColors[3];
                break;
        }

        dressComponent.sprite = finalDress.sprite;
        ribbonComponent.sprite = finalRibbon.sprite;

    }
    public void CompleteOrder()
    {
        Destroy(gameObject);
    }
}
