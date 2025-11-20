using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private bool isOpened = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OpenDoor") && !isOpened)
        {
            isOpened = true;
            StartCoroutine(OpenDoorRotate(collision.gameObject.transform));
        }
    }

    private IEnumerator OpenDoorRotate(Transform door)
    {
        Quaternion startRot = door.rotation;
        Quaternion endRot = door.rotation * Quaternion.Euler(0f, 90f, 0f);

        float duration = 1.2f;
        float time = 0f;

        while (time < duration)
        {
            door.rotation = Quaternion.Slerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        door.rotation = endRot;
    }
}
