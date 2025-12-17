using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GuardPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float waitTime = 1f;

    [Header("Detection")]
    public Transform playerRespawnPoint;

    Vector3 target;
    bool waiting = false;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (waiting) return;

        Vector3 dir = target - transform.position;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir.normalized),
                Time.deltaTime * 5f
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            StartCoroutine(SwapTarget());
        }
    }

    IEnumerator SwapTarget()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        target = (target == pointA.position) ? pointB.position : pointA.position;
        waiting = false;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter : " + other.name);

        if (!other.CompareTag("Player"))
            return;

        RespawnPlayer(other.transform);
    }

    public void RespawnPlayer(Transform player)
    {
        XROrigin xrOrigin = player.GetComponent<XROrigin>();

        if (xrOrigin != null)
        {
            xrOrigin.MoveCameraToWorldLocation(playerRespawnPoint.position);
            xrOrigin.transform.rotation = playerRespawnPoint.rotation;
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = playerRespawnPoint.position;
        player.rotation = playerRespawnPoint.rotation;

        if (cc != null) cc.enabled = true;
    }
}
