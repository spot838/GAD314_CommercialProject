using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CapsuleCollider Collider {  get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterState State { get; private set; }

    protected virtual void Update()
    {
        if (State != null) State.StateTick();
    }

    public bool IsMoving => NavMeshAgent.velocity.magnitude > 0;

    public bool ArrivedAtDestination => Vector3.Distance(transform.position, NavMeshAgent.destination) <= NavMeshAgent.stoppingDistance + 0.001;

    public void SetNewState(CharacterState newState)
    {
        if (State != null) State.StateEnd();
        State = newState;
        State.StateStart();
    }

    public FloorTile OnTile // the tile the character is currently above.
    {
        get
        {
            Vector3 origin = transform.position;
            origin.y += 1f;
            Vector3 direction = -transform.up;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, 5, Game.StationFloorLayer))
            {
                if (hit.collider.TryGetComponent<FloorTile>(out FloorTile floorTile))
                {
                    return floorTile;
                }
            }

            return null;
        }
    }
}
