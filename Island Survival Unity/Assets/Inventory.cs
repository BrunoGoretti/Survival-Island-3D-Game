using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> yourInventory = new List<Item>();

    public int slotsNumber;
    public GameObject x;
    public int n;
    public Image[] slot;
    public Sprite[] slotsSprite;

    void Start()
    {
        //TEST
        yourInventory[0] = Database.itemList[2];
        yourInventory[3] = Database.itemList[2];
    }

    void Update()
    {
        for (int i = 0; i < slotsNumber; i++)
        {
            slot[i].sprite = slotsSprite[i];
        }

        for (int i = 0; i < slotsNumber; i++)
        {
            slotsSprite[i] = yourInventory[i].itemSprite;
        }

        if (PickingUp.y != null)
        {
            x = PickingUp.y;
            n = x.GetComponent<ThisItem>().thisId;
        }
        else
        {
            x = null;
        }

        if(PickingUp.pick == true)
        {
            for(int i = 0; i < slotsNumber; i++)
            {
                if(yourInventory[i].id == 0 && PickingUp.pick == true)
                {
                    yourInventory[i] = Database.itemList[n];
                    PickingUp.pick = false;
                }
            }
            PickingUp.pick = false;
        }
    }
}
