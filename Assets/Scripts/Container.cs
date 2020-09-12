using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [HideInInspector]
    public ItemsCollection itemsCollection;

    [Header("Panels")]
    public GameObject panel;
    public Transform content;

    [Header("slots")]
    public int slotCount = 8;
    public GameObject slotPrefab;

    public List<Slot> slots = new List<Slot>();

    [Header("Items")]
    public List<string> items = new List<string>();

    public Slot slotToMove = null;
    [HideInInspector]
    public bool isOpen = false;


    //      ----- Base Functions -----
    void Start()
    {
        _init();
    }

    public void _init()
    {
        itemsCollection = ItemsCollection.instance;

        items.Clear();
        for (int i = 0; i < slotCount; i++)
        {
            items.Add("");
        }
    }

    void Update()
    {
    }


    //      ----- Custom Functions -----

    public virtual void Open()
    {
        if (isOpen)
        {
            return;
        }

        panel.SetActive(true);
        isOpen = true;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab, content);
            obj.GetComponent<Slot>().itemsCollection = itemsCollection;
            obj.GetComponent<Slot>().container = this;
            obj.GetComponent<Slot>().ChangeItem(items[i]);
            slots.Add(obj.GetComponent<Slot>());
        }
        RefreshContainer();
    }

    public virtual void Close()
    {
        if (!isOpen)
        {
            return;
        }

        panel.SetActive(false);
        isOpen = false;

        foreach (Slot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }


    public virtual void RefreshContainer()
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
    }

    public void SlotToMove(Slot slot)
    {
        slotToMove = slot;
    }

    public virtual void ShiftMoved(Slot slot)
    {

    }

    public virtual void SlotMoved(Slot slot)
    {
        if(slotToMove != null)
        {
            string itemIn = slotToMove.itemId;
            string itemOut = slot.itemId;

            AddItem(itemOut, slotToMove);
            AddItem(itemIn, slot);

            slotToMove = null;
        }
    }

    public void ResetSlotToMove(Slot slot)
    {
        
        if (slot == slotToMove)
        {
            slotToMove = null;
            GameObject toInstantiate;
            if ((toInstantiate = itemsCollection.GetItemObject(slot.itemId)))
            {
                GameObject obj = Instantiate(toInstantiate);
                obj.transform.position = this.transform.position + new Vector3(1, 0, 0);
            }
            AddItem("", slot);
        }
    }

    public virtual bool AddItem(string id)
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (!itemsCollection.ItemExist(items[i]))
            {
                items[i] = id;
                RefreshContainer();
                return true;
            }
        }
        return false;
    }

    public virtual void AddItem(string id, Slot slot)
    {
        int pos = GetSlotPos(slot);
        items[pos] = id;
        RefreshContainer();
    }

    public int GetSlotPos(Slot slotToFind)
    {
        int index = 0;

        foreach(Slot slot in slots)
        {
            if(slot == slotToFind)
            {
                return index;
            }
            index++;
        }


        return -1;
    }
}
