using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingBag : MonoBehaviour
{
    public int Id; 
    public SpriteRenderer Front;
    public SpriteRenderer Back;

    public void Init(Sprite sprite)
    {
        Front.sprite = sprite;
        Back.sprite = sprite;
    }
}
