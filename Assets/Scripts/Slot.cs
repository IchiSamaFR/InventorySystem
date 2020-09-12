using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public ItemsCollection itemsCollection;


    public GameObject moveObjPrefab;
    GameObject moveObj;

    public Container container;
    public string itemId;

    bool isOver = false;


    // Start is called before the first frame update
    void Start()
    {
        itemsCollection = ItemsCollection.instance;
    }

    // Update is called once per frame
    void Update()
    {
        MouseOver();


        if (isOver)
        {
            if (Input.GetMouseButtonDown(0) && itemsCollection.ItemExist(itemId))
            {
                if(Input.GetKey("left shift"))
                {
                    container.ShiftMoved(this);
                }
                else
                {
                    container.SlotToMove(this);
                    moveObj = Instantiate(moveObjPrefab, GameObject.Find("Canvas").transform);
                    moveObj.transform.GetChild(0).GetComponent<Image>().sprite = itemsCollection.GetItemSprite(itemId);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                container.SlotMoved(this);
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine("MouseUp");
            }
        }

        if (moveObj)
        {
            moveObj.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            if (!Input.GetMouseButton(0))
            {
                Destroy(moveObj);
            }
        }


    }

    public IEnumerator MouseUp()
    {
        yield return new WaitForEndOfFrame();
        container.ResetSlotToMove(this);
    }


    void MouseOver()
    {
        if (Input.mousePosition.x > this.transform.position.x - this.transform.GetComponent<RectTransform>().sizeDelta.x / 2
            && Input.mousePosition.x < this.transform.position.x + this.transform.GetComponent<RectTransform>().sizeDelta.x / 2
            && Input.mousePosition.y > this.transform.position.y - this.transform.GetComponent<RectTransform>().sizeDelta.y / 2
            && Input.mousePosition.y < this.transform.position.y + this.transform.GetComponent<RectTransform>().sizeDelta.y / 2)
        {
            isOver = true;
        }
        else
        {
            isOver = false;
        }
    }

    public void ChangeItem(string id)
    {
        if (itemsCollection.ItemExist(id))
        {
            itemId = id;
            transform.GetChild(0).GetComponent<Image>().enabled = true;
            transform.GetChild(0).GetComponent<Image>().sprite = itemsCollection.GetItemSprite(id);
        }
        else
        {
            itemId = "";
            transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }
}
