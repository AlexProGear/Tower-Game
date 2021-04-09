using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Camera mainCamera;
    private Animator animator;
    private NavMeshAgent navAgent;
    
    private bool hasGun;
    private Quaternion targetRotation;
    private Coroutine cooldownCoroutine;
    private float shootCooldownTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Ray lookDirRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (hasGun && Physics.Raycast(lookDirRay, out RaycastHit hitInfo))
        {
            UpdateGunAiming(hitInfo.point);
            if (Input.GetMouseButton(0))
            {
                UpdateShooting(hitInfo.point);
            }
        }
    }

    private void UpdateGunAiming(Vector3 hitPoint)
    {
        // Calculate new rotation
        Vector3 aimDirection = hitPoint - transform.position;
        Vector3 aimDirectionHorizontal = new Vector3(aimDirection.x, 0, aimDirection.z);
        targetRotation = Quaternion.LookRotation(aimDirectionHorizontal);
        // Update rotation
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Lerp(current, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateShooting(Vector3 hitPoint)
    {
        if (shootCooldownTime <= 0)
        {
            // TODO Spawn projectile
            Debug.Log("Pew!");
            
            if (cooldownCoroutine != null)
                StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        shootCooldownTime = 1f; // Arbitrary cooldown time
        while (shootCooldownTime > 0)
        {
            shootCooldownTime -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (hasGun)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.9f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + transform.forward + Vector3.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GunSpot"))
        {
            hasGun = true;
            navAgent.updateRotation = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GunSpot"))
        {
            hasGun = false;
            navAgent.updateRotation = true;
        }
    }
}
