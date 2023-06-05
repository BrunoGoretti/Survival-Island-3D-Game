using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisItem : MonoBehaviour
{
    public List<Item> thisItem = new List<Item>();

    public int thisId;
    public int id;
    public string itemName;
    public string itemDescription;

    void Start()
    {
        thisItem[0] = Database.itemList[thisId];
    }

    void Update()
    {
        id = thisItem[0].id;
        itemName = thisItem[0].name;
        itemDescription = thisItem[0].description;
    }
}
