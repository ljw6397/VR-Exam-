using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyHoleTrigger : MonoBehaviour
{
    public Transform keyInsertPoint;
    public Transform door;           
    private bool unlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (unlocked) return;

        if (other.CompareTag("Key"))
        {
            unlocked = true;
            StartCoroutine(OpenSequence(other.transform));
        }
    }

    private IEnumerator OpenSequence(Transform key)
    {
        XRGrabInteractable grab = key.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected)
        {
            grab.interactionManager.SelectExit(grab.firstInteractorSelecting, grab);
        }

        Rigidbody rb = key.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Quaternion savedWorldRot = key.rotation;

        key.SetParent(keyInsertPoint);

        key.rotation = savedWorldRot;

        float snapTime = 0.3f;
        float t = 0;
        Vector3 startPos = key.position;
        Quaternion startRot = key.rotation;

        while (t < snapTime)
        {
            key.position = Vector3.Lerp(startPos, keyInsertPoint.position, t / snapTime);
            key.rotation = Quaternion.Slerp(startRot, keyInsertPoint.rotation, t / snapTime);
            t += Time.deltaTime;
            yield return null;
        }

        key.localPosition = Vector3.zero;
        key.localRotation = Quaternion.identity;   

        yield return new WaitForSeconds(1f);

        Destroy(key.gameObject);

        Quaternion doorStart = door.rotation;
        Quaternion doorEnd = door.rotation * Quaternion.Euler(0, 90f, 0);
        float doorTime = 1.2f;
        t = 0;

        while (t < doorTime)
        {
            door.rotation = Quaternion.Slerp(doorStart, doorEnd, t / doorTime);
            t += Time.deltaTime;
            yield return null;
        }

        door.rotation = doorEnd;
    }

}
