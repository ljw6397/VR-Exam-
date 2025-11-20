using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openSpeed = 2f;       
    public float openAngle = 90f;      

    private Quaternion closedRot;
    private Quaternion openRot;
    private bool opened = false;

    private void Start()
    {
        closedRot = transform.rotation;
        openRot = transform.rotation * Quaternion.Euler(0, -openAngle, 0);
    }

    public void OpenDoor()
    {
        if (!opened)
            StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        opened = true;

        Collider col = GetComponent<Collider>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (col != null) col.enabled = false;
        if (rb != null) rb.isKinematic = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.rotation = Quaternion.Slerp(closedRot, openRot, t);
            yield return null;
        }

        transform.rotation = openRot;
    }
}
