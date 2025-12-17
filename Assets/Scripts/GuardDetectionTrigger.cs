using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDetectionTrigger : MonoBehaviour
{
    public GuardPatrol guard;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        guard.RespawnPlayer(other.transform);
    }
}
