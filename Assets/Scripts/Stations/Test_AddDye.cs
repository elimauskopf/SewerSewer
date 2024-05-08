using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test_AddDye : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player entering zone");
        if (!collision.transform.CompareTag(Tags.Player))
        {
            return;
        }

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }

        int randomMax = Enum.GetValues(typeof(ColorTypes)).Length;
        ColorTypes color = (ColorTypes)UnityEngine.Random.Range(1, randomMax - 1);

        playerController.AssignItem(ItemTypes.Dye, color);
        Debug.Log("Gave player dye");
    }
}
