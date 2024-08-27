using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static List<Item> itemList = new List<Item>();

    void Awake ()
    {
        itemList.Add(new Item(0, "None", "None", Resources.Load <Sprite>("0")));
        itemList.Add(new Item(1, "Stick", "It is item", Resources.Load <Sprite>("1")));
        itemList.Add(new Item(2, "Stone", "It is item", Resources.Load <Sprite>("2")));
        itemList.Add(new Item(3, "Coconut", "It is item", Resources.Load <Sprite>("3")));
    }
}
