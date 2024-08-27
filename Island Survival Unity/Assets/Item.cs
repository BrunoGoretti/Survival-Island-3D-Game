using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Item 
{
    public int id;
    public string name; 
    public string description;

    public Sprite itemSprite;

    public int stack; 

    public Item()
    {

    }

    public Item(int Id, string Name, string Description, Sprite ItemSprite, int Stack)
    {
        id = Id;
        name = Name;
        description = Description;
        itemSprite = ItemSprite;
        stack = Stack;
    }
}
 