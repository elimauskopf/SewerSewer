using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObject : MonoBehaviour
{
    SpriteRenderer _renderer;

    ItemTypes _type;
    ColorTypes? _color;

    Dictionary<ItemTypes, ColorTypes> _subContents = new Dictionary<ItemTypes, ColorTypes>();

    public ItemTypes ItemType { get { return _type; } }
    public ColorTypes? ColorType { get { return _color; } }

    //sprite type for each type
    //List of sprites color order follow list of ColorTypes --> 0:White, 1:Red, 2:Green, 3:Yellow
    public Sprite fish;

    public List<Sprite> silk = new List<Sprite>();
    public List<Sprite> fabric = new List<Sprite>();
    public List<Sprite> dress = new List<Sprite>();
    public List<Sprite> ribbon = new List<Sprite>();
    public List<Sprite> dye = new List<Sprite>();
   

    public ItemObject(ItemTypes itemType, ColorTypes? colorType)
    {
        _type = itemType;
        _color = colorType;
    }

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
        _renderer.sprite = ChooseSprite(_type, _color);
    }

    public Sprite ChooseSprite(ItemTypes? item, ColorTypes? color)
    {
        if(color.Equals(ColorTypes.None))
        {
            return null;
        }

        switch (item)
        {
            case ItemTypes.None:
                return null;
            case ItemTypes.Fish:            
                return fish; ;
            case ItemTypes.Silk:
                return silk[(int)color];
            case ItemTypes.Fabric:             
                return fabric[(int)color];
            case ItemTypes.Dress:
                return dress[(int)color];
            case ItemTypes.Ribbon:
                return ribbon[(int)color];
            case ItemTypes.Dye:
                return dye[(int)color];
            default:
                return null;
            
        }
    }
}
