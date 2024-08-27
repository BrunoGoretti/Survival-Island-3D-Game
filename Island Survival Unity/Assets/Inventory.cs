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

    public int[] slotStack;
    public int maxStack;

    public int slotTemporary;

    public int rest;
    public bool shift;

    public bool canConsume;

    void Start()
    {
        yourInventory[1] = Database.itemList[3];
        slotStack[1] = 3;
        yourInventory[2] = Database.itemList[3];
        slotStack[2] = 2;
    }

    void Update()
    {
        if(Input.GetKeyDown("left shift"))
        {
            shift = true;
        }

        if (Input.GetKeyUp("left shift"))
        {
            shift = false;
        }

        for (int i = 0; i < slotsNumber; i++)
        {
            if (yourInventory[i].id == 0)
            {
                stackText[i].text = "";
            }
            else
            {
                stackText[i].text = "" + slotStack[i];
            }
        }

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
                    if (slotStack[i] == maxStack)
                    {
                        continue;
                    }
                    else
                    {
                        slotStack[i] += 1;
                        i = slotsNumber;
                        PickingUp.pick = false;
                    }
                }
            }

            for (int i = 0; i < slotsNumber; i++)
            {
                if (yourInventory[i].id == 0 && PickingUp.pick == true)
                {
                    yourInventory[i] = Database.itemList[n];
                    slotStack[i] += 1;
                    PickingUp.pick = false;
                }
            }
            PickingUp.pick = false;
        }

        if (yourInventory[b].consumable == true)
        {
            canConsume = true;
        }
        else
        {
            canConsume = false;
        }

        if (canConsume == true && Input.GetMouseButtonDown(1))
        {
            if (slotStack[b] == 1)
            {
                PlayerStats.UpdatedHunger += yourInventory[b].nutritionaValue;
                yourInventory[b] = Database.itemList[0];
                slotStack[b] = 0;
            }
            else
            {

                slotStack[b] --;
                PlayerStats.UpdatedHunger += yourInventory[b].nutritionaValue;
            }
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
        if (shift == true)
        {
            if (yourInventory[b].id == 0 && slotStack[a] >= 2)
            {
                yourInventory[b] = yourInventory[a];
                slotStack[b] = slotStack[a] / 2;
                rest = slotStack[a] % 2;
                slotStack[a] = slotStack[a] / 2 + rest;
            }
        }
        else
        {
            print("stop drag: " + slotX.name);

            if (a != b)
            {
                if (yourInventory[a].id != yourInventory[b].id)
                {

                    draggedItem[0] = yourInventory[a];
                    slotTemporary = slotStack[a];
                    yourInventory[a] = yourInventory[b];
                    slotStack[a] = slotStack[b];
                    yourInventory[b] = draggedItem[0];
                    slotStack[b] = slotTemporary;
                    a = 0;
                    b = 0;
                }
                else
                {
                    if (slotStack[a] + slotStack[b] <= maxStack)
                    {
                        slotStack[b] = slotStack[a] + slotStack[b];
                        yourInventory[a] = Database.itemList[0];
                    }
                    else
                    {
                        slotStack[a] = slotStack[a] + slotStack[b] - maxStack;
                        slotStack[b] = maxStack;
                    }
                }
            }
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
