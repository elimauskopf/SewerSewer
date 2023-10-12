using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObject : ScriptableObject
{
    public Sprite icon;
    public ItemTypes type;
}
