using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static List<Item> itemList = new List<Item>()
    {
        new Item(0, "None", "None"),
        new Item(1, "Stick", "It is item"),
        new Item(2, "Stone", "It is item"),
        new Item(3, "Fruit", "It is item")
    };
}
