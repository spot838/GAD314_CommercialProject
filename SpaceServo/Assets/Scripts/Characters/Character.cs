using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CapsuleCollider Collider {  get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [SerializeField] float stoppingDistanceAnimationOffset = 0.1f;

    protected virtual void Update()
    {
        Animator.SetBool("moving", IsMoving);
    }

    public bool IsMoving => NavMeshAgent.velocity.magnitude > 0 && Vector3.Distance(transform.position, NavMeshAgent.destination) > NavMeshAgent.stoppingDistance + stoppingDistanceAnimationOffset;

    public bool HasArrivedAtNavMeshDestination => Vector3.Distance(transform.position, NavMeshAgent.destination) <= NavMeshAgent.stoppingDistance + stoppingDistanceAnimationOffset;
}
