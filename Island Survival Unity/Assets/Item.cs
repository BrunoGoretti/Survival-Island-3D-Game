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

    public Item()
    {

    }

    public Item(int Id, string Name, string Description)
    {
        id = Id;
        name = Name;
        description = Description;
    }
}
