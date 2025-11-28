using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FreezableAgent : MonoBehaviour
{
    public bool IsFrozen { get; private set; }
    Animator anim; NavMeshAgent agent; Rigidbody rb;
    float savedAnimSpeed = 1f; bool hadAgent;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        hadAgent = agent != null && agent.enabled;
    }

    public void ApplyFreeze(float duration)
    {
        if (gameObject.activeInHierarchy) StartCoroutine(FreezeCo(duration));
    }

    IEnumerator FreezeCo(float t)
    {
        IsFrozen = true;
        if (anim) { savedAnimSpeed = anim.speed; anim.speed = 0f; }
        if (agent) { agent.isStopped = true; }
        if (rb) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        // ¿¹: GetComponent<AllyUnit>()?.SetActionBlocked(true);

        yield return new WaitForSeconds(t);

        if (anim) anim.speed = savedAnimSpeed;
        if (agent && hadAgent) agent.isStopped = false;

        // ¿¹: GetComponent<AllyUnit>()?.SetActionBlocked(false);

        IsFrozen = false;
    }
}
