using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public int speed;


    void Update()
    {
        if (Input.GetKey("d"))
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("q"))
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown("i"))
        {
            if (this.GetComponent<Container>().isOpen)
            {
                this.GetComponent<Container>().Close();
            }
            else
            {
                this.GetComponent<Container>().Open();
            }
        }

        if (Input.GetKeyDown("x"))
        {
            this.GetComponent<Container>().AddItem("milk");
        }
    }
}
