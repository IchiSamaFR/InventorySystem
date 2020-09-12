using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Container
{
    [System.Serializable]
    public struct item
    {
        public string id;
        public int chance;
    }

    Inventory inventory;
    public List<item> toGen = new List<item>();



    // Start is called before the first frame update
    void Start()
    {
        _init();
        RandomGen();
    } 

    void RandomGen()
    {
        for (int i = 0; i < slotCount; i++)
        {
            int rdm = Random.Range(0, 100);
            int count = 0;
            foreach (item itemStruct in toGen)
            {
                count += itemStruct.chance;
                if(rdm < count)
                {
                    items[i] = itemStruct.id;
                    break;
                }
            }
        }
    }

    public override void Open()
    {
        if (isOpen)
        {
            return;
        }

        if (inventory.chest != this && inventory.chest != null)
        {
            inventory.chest.Close();
        }

        inventory.chest = this;
        inventory.Open();

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

    public override void Close()
    {
        if (!isOpen)
        {
            return;
        }

        if(inventory.chest != null && inventory.chest == this)
        {
            inventory.chest = null;
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
        else if (inventory.slotToMove != null)
        {
            string itemIn = inventory.slotToMove.itemId;
            string itemOut = slot.itemId;

            inventory.AddItem(itemOut, inventory.slotToMove);
            AddItem(itemIn, slot);

            inventory.slotToMove = null;
        }
    }

    public override void ShiftMoved(Slot slot)
    {
        int pos = GetSlotPos(slot);
        if (inventory.AddItem(items[pos]))
        {
            AddItem("", slot);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (Input.GetKeyDown("e"))
            {
                if (isOpen)
                {
                    Close();
                }
                else
                {
                    inventory = coll.GetComponent<Inventory>();
                    Open();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Close();
        }
    }
}
