using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openSpeed = 3f;
    public float openHeight = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool opened = false;

    private void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + new Vector3(0, openHeight, 0);
    }

    public void OpenDoor()
    {
        if (!opened)
            StartCoroutine(Open());
    }

    private System.Collections.IEnumerator Open()
    {
        opened = true;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * openSpeed;
            transform.position = Vector3.Lerp(closedPos, openPos, t);
            yield return null;
        }
    }
}
