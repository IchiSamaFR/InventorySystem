using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection : MonoBehaviour
{

    public static ItemsCollection instance;
    public List<Item> items;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool ItemExist(string id)
    {
        foreach(Item item in items)
        {
            if(item.id == id)
            {
                return true;
            }
        }


        return false;
    }

    public Sprite GetItemSprite(string id)
    {
        foreach (Item item in items)
        {
            if (item.id == id)
            {
                return item.sprite;
            }
        }

        return null;
    }
    public GameObject GetItemObject(string id)
    {
        foreach (Item item in items)
        {
            if (item.id == id)
            {
                return item.groundItem;
            }
        }

        return null;
    }
}


[System.Serializable]
public struct Item
{
    public string id;
    public Sprite sprite;
    public GameObject groundItem;
}
