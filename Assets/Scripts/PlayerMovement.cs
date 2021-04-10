using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent navAgent;
    private Animator animator;
    private static readonly int SpeedPropertyID = Animator.StringToHash("Speed");

    private void Start()
    {
        // Caching necessary components
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        ApplySettings();
    }

    private void ApplySettings()
    {
        PlayerSettings settings = Resources.Load<PlayerSettings>("PlayerSettings");
        navAgent.speed = settings.MovementSpeed;
    }

    private void Update()
    {
        // Set new destination on click
        if (Input.GetMouseButtonDown(1))
        {
            Ray lookDirRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(lookDirRay, out RaycastHit hitInfo))
            {
                navAgent.SetDestination(hitInfo.point);
            }
        }
        
        // Update animation values
        float movementSpeed = navAgent.desiredVelocity.magnitude;
        animator.SetFloat(SpeedPropertyID, movementSpeed);
    }
}
