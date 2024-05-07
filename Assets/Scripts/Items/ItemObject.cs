using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObject : MonoBehaviour
{
    SpriteRenderer _renderer;

    ItemTypes _type;
    ColorTypes _color;

    public ItemTypes ItemType { get { return _type; } }
    public ColorTypes ColorType { get { return _color; } }

    //sprite type for each type
    //List of sprites color order follow list of ColorTypes --> 0:White, 1:Red, 2:Green, 3:Yellow
    public Sprite fish;

    public List<Sprite> silk = new List<Sprite>();
    public List<Sprite> fabric = new List<Sprite>();
    public List<Sprite> dress = new List<Sprite>();
    public List<Sprite> ribbon = new List<Sprite>();

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void AssignItemType(ItemTypes item)
    {
        _type = item;
        AssignSprite();
    }
    public void AssignColor(ColorTypes newColor)
    {
        _color = newColor;
        AssignSprite();
    }

    void AssignSprite()
    {
        switch (_type)
        {
            case ItemTypes.None:
                return;
            case ItemTypes.Fish:
                _renderer.sprite = fish;
                break;
            case ItemTypes.Silk:
                _renderer.sprite = silk[(int)_color];
                break;
            case ItemTypes.Fabric:
                _renderer.sprite = fabric[(int)_color];
                break;
            case ItemTypes.Dress:
                _renderer.sprite = dress[(int)_color];
                break;
            case ItemTypes.Ribbon:
                _renderer.sprite = ribbon[(int)_color];
                break;
            default:
                break;
        }
    }
}
