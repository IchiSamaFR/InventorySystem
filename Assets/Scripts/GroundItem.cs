using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public string id;

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (coll.GetComponent<Inventory>().AddItem(id))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
