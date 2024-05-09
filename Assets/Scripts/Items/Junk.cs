using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{

    public ItemTypes itemType;
    public ColorTypes colorType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Tags.Ground)
        {
            Destroy(gameObject);
        }
    }
}
