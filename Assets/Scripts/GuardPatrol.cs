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
    public GuardDetectionTrigger detectionTrigger; 


    [Header("Death")]
    public Animator animator;
    bool isDead = false;

    [Header("Drop")]
    public GameObject keyPrefab;
    public Vector3 keyDropOffset = new Vector3(0.3f, 0.2f, 0f);

    public AudioSource audioSource;
    public AudioClip keyDropSound;
    public AudioClip hammerHitSound;

    [Header("Footstep Sound")]
    public AudioClip footstepSound;
    public float footstepInterval = 0.5f; // 발소리 간격
    float footstepTimer = 0f;

    Vector3 target;
    bool waiting = false;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        if (isDead) return;

        Patrol();
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        if (waiting) return;

        // 실제로 움직이고 있을 때만
        if (Vector3.Distance(transform.position, target) > 0.05f)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }
    }

    void PlayFootstep()
    {
        if (audioSource && footstepSound)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(footstepSound, 0.6f);
            audioSource.pitch = 1f;
        }
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
        if (isDead) return;

        Debug.Log("Hit : " + other.name + " / tag = " + other.tag);

        if (other.CompareTag("Hammer"))
        {
            PlayHammerHitSound(); 
            Die();
            return;
        }

        if (other.CompareTag("Player"))
        {
            RespawnPlayer(other.transform);
        }
    }
    void Die()
    {
        isDead = true;

        animator.SetTrigger("Death");

        StopAllCoroutines();

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (detectionTrigger != null)
            detectionTrigger.gameObject.SetActive(false);

        DropKey();
    }

    void DropKey()
    {
        if (keyPrefab == null) return;

        Vector3 dropPos =
            transform.position +
            transform.right * keyDropOffset.x +
            Vector3.up * keyDropOffset.y +
            transform.forward * keyDropOffset.z;

        Instantiate(keyPrefab, dropPos, Quaternion.identity);

        if (audioSource && keyDropSound)
            audioSource.PlayOneShot(keyDropSound);
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

    void PlayHammerHitSound()
    {
        if (audioSource && hammerHitSound)
            audioSource.PlayOneShot(hammerHitSound);
    }
}
