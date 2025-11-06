using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OpenDoor"))
        {
            Destroy(collision.gameObject  , 1.5f);
        }
    }
}
