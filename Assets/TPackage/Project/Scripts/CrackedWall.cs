using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    public GameObject[] wallParts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Hammer"))
            {
                foreach (GameObject obj in wallParts)
                {
                    if (obj.GetComponent<Rigidbody>() == null)
                        obj.AddComponent<Rigidbody>();
                }
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject != null)
    //    {
    //        if (collision.gameObject.CompareTag("Hammer"))
    //        {
    //            gameObject.GetComponent<Collider>().isTrigger = true;

    //            foreach (GameObject obj in wallParts)
    //            {
    //                if (obj.GetComponent<Rigidbody>() == null)
    //                    obj.AddComponent<Rigidbody>();
    //            }
    //        }
    //    }
    //}
}
