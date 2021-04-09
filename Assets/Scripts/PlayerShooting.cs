using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerShooting : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private Transform gunHoldingPoint;
    [SerializeField] private float rotationSpeed;

    // Unity components
    private Camera mainCamera;
    private Animator animator;
    private NavMeshAgent navAgent;
    
    // Private fields
    private bool canWieldGun;
    private WeaponSystem.GunType equippedGunType = WeaponSystem.GunType.None;
    [SerializeField] private WeaponSystem.GunType lastEquippedGunType = WeaponSystem.GunType.Pistol;
    private GunObject equippedGunObject;
    private GameObject equippedGunGameObject;
    
    private Quaternion targetRotation;
    private Coroutine cooldownCoroutine;
    private float shootCooldownTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        EquipGun(WeaponSystem.GunType.None);
    }

    private void Update()
    {
        Ray lookDirRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (canWieldGun && Physics.Raycast(lookDirRay, out RaycastHit hitInfo))
        {
            UpdateGunAiming(hitInfo.point);
            if (Input.GetMouseButton(0))
            {
                UpdateShooting(hitInfo.point);
            }
        }
    }

    private void EquipGun(WeaponSystem.GunType type)
    {
        if (equippedGunType == type)
            return;
        
        equippedGunType = type;
        
        if (equippedGunGameObject != null)
            Destroy(equippedGunGameObject);
        
        if (type == WeaponSystem.GunType.None)
            return;

        equippedGunObject = WeaponSystem.Instance.GunObjectsDict[type];
        GameObject gunPrefab = WeaponSystem.Instance.GunPrefabsDict[equippedGunObject.PrefabName];
        equippedGunGameObject = Instantiate(gunPrefab, gunHoldingPoint);
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
        shootCooldownTime = equippedGunObject.ShotsInterval;
        while (shootCooldownTime > 0)
        {
            shootCooldownTime -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (canWieldGun)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.9f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, transform.position + transform.forward + Vector3.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GunSpot"))
        {
            canWieldGun = true;
            navAgent.updateRotation = false;
            EquipGun(lastEquippedGunType);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GunSpot"))
        {
            canWieldGun = false;
            navAgent.updateRotation = true;
            EquipGun(WeaponSystem.GunType.None);
        }
    }
}
