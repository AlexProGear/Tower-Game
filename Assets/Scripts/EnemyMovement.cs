using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float updateTargetInterval = 0.5f;
    
    private Transform playerTransform;
    private NavMeshAgent navAgent;
    private Animator animator;
    private static readonly int MovePropertyID = Animator.StringToHash("Move");

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        StartCoroutine(UpdateTargetPositionPeriodically());
    }

    private IEnumerator UpdateTargetPositionPeriodically()
    {
        while (enabled)
        {
            navAgent.SetDestination(playerTransform.position);
            yield return new WaitForSeconds(updateTargetInterval);
        }
    }

    private void Update()
    {
        // Update animation values
        float movementSpeed = navAgent.desiredVelocity.magnitude;
        animator.SetBool(MovePropertyID, movementSpeed > Mathf.Epsilon);
    }
}
