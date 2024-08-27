using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> yourInventory = new List<Item>();

    public List<Item> draggedItem = new List<Item>();

    public int slotsNumber;
    public GameObject x;
    public int n;

    public Image[] slot;
    public Sprite[] slotsSprite;
    public Text[] stackText;

    public int a;
    public int b;

    void Start()
    {
        yourInventory[1] = Database.itemList[2];
        yourInventory[1].stack += 4;
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

        if (PickingUp.pick == true)
        {
            for (int i = 0; i < slotsNumber; i++)
            {
                if (yourInventory[i].id == n)
                {
                    yourInventory[i].stack += 1;
                    i = slotsNumber;
                    PickingUp.pick = false;
                }
            }

            for (int i = 0; i < slotsNumber; i++)
            {
                if (yourInventory[i].id == 0 && PickingUp.pick == true)
                {
                    yourInventory[i] = Database.itemList[n];
                    yourInventory[i].stack += 1;
                    PickingUp.pick = false;
                }
            }
            PickingUp.pick = false;
        }

        for (int i = 0; i < slotsNumber; i++)
        {
            stackText[i].text = "" + yourInventory[i].stack;
        }
    }

    public void StartDrag(Image slotX)
    {
        print("start drag: " + slotX.name);

        for (int i = 0; i < slotsNumber; i++)
        {
            if (slot[i] == slotX)
            {
                a = i;
            }
        }
    }

    public void Drop(Image slotX) 
    {
        print("stop drag: " + slotX.name);

        if (a != b)
        {
            draggedItem[0] = yourInventory[a];
            yourInventory[a] = yourInventory[b];
            yourInventory[b] = draggedItem[0];
            a = 0;
            b = 0;
        }
    }

    public void Enter(Image slotX)
    {
        print("enter");

        for (int i = 0; i < slotsNumber; i++)
        {
            if (slot[i] == slotX)
            {
                b = i;
            }
        }
    }
}
