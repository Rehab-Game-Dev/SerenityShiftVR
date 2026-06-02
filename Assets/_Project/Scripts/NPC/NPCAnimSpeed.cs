using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCAnimSpeed : MonoBehaviour
{
    public Animator animator;
    public string speedParam = "speed";
    public float damp = 0.12f;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (animator) animator.applyRootMotion = false;
    }

    void Update()
    {
        if (!animator) return;
        float v = agent.velocity.magnitude;
        animator.SetFloat(speedParam, v, damp, Time.deltaTime);
    }
}
