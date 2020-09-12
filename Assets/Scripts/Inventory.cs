using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Container
{
    public Container chest;


    [Header("Bar Items")]
    public Transform contentBar;
    public int slotBarCount = 2;
    public List<Slot> slotsBar = new List<Slot>();
    public List<string> itemsBar = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        _init();

        itemsBar.Clear();
        for (int i = 0; i < slotBarCount; i++)
        {
            itemsBar.Add("");

            GameObject obj = Instantiate(slotPrefab, contentBar);
            obj.GetComponent<Slot>().itemsCollection = itemsCollection;
            obj.GetComponent<Slot>().container = this;
            obj.GetComponent<Slot>().ChangeItem(itemsBar[i]);
            slotsBar.Add(obj.GetComponent<Slot>());
        }
    }
    public override void Close()
    {
        if (!isOpen)
        {
            return;
        }
        if(chest != null)
        {
            chest.Close();
        }

        panel.SetActive(false);
        isOpen = false;

        foreach (Slot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }


    public override void SlotMoved(Slot slot)
    {
        if (slotToMove != null)
        {
            string itemIn = slotToMove.itemId;
            string itemOut = slot.itemId;

            AddItem(itemOut, slotToMove);
            AddItem(itemIn, slot);

            slotToMove = null;
        }
        else if (chest && chest.slotToMove != null)
        {
            string itemIn = chest.slotToMove.itemId;
            string itemOut = slot.itemId;

            chest.AddItem(itemOut, chest.slotToMove);
            AddItem(itemIn, slot);

            chest.slotToMove = null;
        }
    }

    public override void ShiftMoved(Slot slot)
    {
        if (!isOpen)
        {
            return;
        }
        if (chest)
        {
            int pos = GetSlotPos(slot);
            if (pos >= 0)
            {
                if (chest.AddItem(items[pos]))
                {
                    AddItem("", slot);
                }
            }
            else
            {
                pos = GetSlotBarPos(slot);
                if (chest.AddItem(itemsBar[pos]))
                {
                    AddItem("", slot);
                }
            }
        }
        else
        {
            int pos = GetSlotPos(slot);
            if (pos >= 0)
            {
                if (AddItemBar(items[pos]))
                {
                    AddItem("", slot);
                }
            }
            else
            {
                pos = GetSlotBarPos(slot);
                if (AddItem(itemsBar[pos]))
                {
                    AddItem("", slot);
                }
            }
        }
    }
    public override void RefreshContainer()
    {
        int index = 0;
        foreach (Slot slot in slots)
        {
            if (slot)
            {
                slot.itemsCollection = itemsCollection;
                slot.ChangeItem(items[index]);
            }
            index++;
        }

        index = 0;
        foreach (Slot slot in slotsBar)
        {
            if (slot)
            {
                slot.itemsCollection = itemsCollection;
                slot.ChangeItem(itemsBar[index]);
            }
            index++;
        }
    }
    public override void AddItem(string id, Slot slot)
    {
        int pos = GetSlotPos(slot);
        if(pos >= 0)
        {
            items[pos] = id;
            RefreshContainer();
        }
        else
        {
            pos = GetSlotBarPos(slot);
            itemsBar[pos] = id;
            RefreshContainer();
        }
    }
    public bool AddItemBar(string id)
    {
        for (int i = 0; i < slotBarCount; i++)
        {
            if (!itemsCollection.ItemExist(itemsBar[i]))
            {
                itemsBar[i] = id;
                RefreshContainer();
                return true;
            }
        }
        return false;
    }

    public int GetSlotBarPos(Slot slotToFind)
    {
        int index = 0;

        foreach (Slot slot in slotsBar)
        {
            if (slot == slotToFind)
            {
                return index;
            }
            index++;
        }


        return -1;
    }
}
