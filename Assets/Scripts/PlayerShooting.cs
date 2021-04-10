using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerShooting : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private Transform gunHoldingPoint;
    [SerializeField] private Transform shoulderPoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private LayerMask aimMask;

    // Unity components
    private Camera mainCamera;
    private Animator animator;
    private NavMeshAgent navAgent;
    
    // Private fields
    private bool canWieldGun;
    private WeaponSystem.GunType equippedGunType = WeaponSystem.GunType.None;
    private WeaponSystem.GunType lastEquippedGunType = WeaponSystem.GunType.Pistol;
    private GunTypeInfo equippedGunTypeInfo;
    private GameObject equippedGunItem;
    private Transform gunShotPoint;
    private GameObject shellPrefab;
    private ShellTypeInfo shellTypeInfo;
    
    private Quaternion targetRotation;
    private Coroutine cooldownCoroutine;
    private float shootCooldownTime = 0f;

    private Vector3 aimPointPosition;

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
        if (canWieldGun && Physics.Raycast(lookDirRay, out RaycastHit hitInfo, 100f, aimMask))
        {
            aimPointPosition = hitInfo.point;
            UpdateGunAiming();
            if (Input.GetMouseButton(0))
            {
                UpdateShooting();
            }
            UpdateWeaponSelection();
        }
    }

    private void UpdateWeaponSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipGun(WeaponSystem.GunType.Pistol);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipGun(WeaponSystem.GunType.Shotgun);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            EquipGun(WeaponSystem.GunType.Machinegun);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            EquipGun(WeaponSystem.GunType.Uzi);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            EquipGun(WeaponSystem.GunType.RPG);
    }

    private void EquipGun(WeaponSystem.GunType type)
    {
        if (equippedGunType == type)
            return;
        
        equippedGunType = type;
        
        if (equippedGunItem != null)
            Destroy(equippedGunItem);
        
        if (type == WeaponSystem.GunType.None)
            return;

        equippedGunTypeInfo = WeaponSystem.Instance.GetGunObject(type);
        GameObject gunPrefab = WeaponSystem.Instance.GetGunPrefab(equippedGunTypeInfo.PrefabName);
        equippedGunItem = Instantiate(gunPrefab, gunHoldingPoint);
        gunShotPoint = equippedGunItem.FindChildWithTag("ShotPoint").transform;
        shellTypeInfo = WeaponSystem.Instance.GetShellObject(equippedGunTypeInfo.ShellType);
        shellPrefab = WeaponSystem.Instance.GetShellPrefab(shellTypeInfo.PrefabName);
    }

    private void UpdateGunAiming()
    {
        // Calculate new rotation
        Vector3 aimDirection = aimPointPosition - transform.position;
        Vector3 aimDirectionHorizontal = new Vector3(aimDirection.x, 0, aimDirection.z);
        targetRotation = Quaternion.LookRotation(aimDirectionHorizontal);
        // Update rotation
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Lerp(current, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateShooting()
    {
        if (shootCooldownTime <= 0)
        {
            // Spawn bullets
            for (int i = 0; i < equippedGunTypeInfo.BulletsPerShot; i++)
            {
                SpawnBullet();
            }

            if (cooldownCoroutine != null)
                StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = StartCoroutine(CooldownCoroutine());
        }
    }

    private void SpawnBullet()
    {
        Vector3 direction = (aimPointPosition - gunShotPoint.position).normalized;
        direction += Random.onUnitSphere * equippedGunTypeInfo.BulletSpread;
        Quaternion bulletDirection = Quaternion.LookRotation(direction);
        
        GameObject newBullet = Instantiate(shellPrefab, gunShotPoint.position, bulletDirection);
        newBullet.GetComponent<BulletScript>().Setup(shellTypeInfo);
    }

    private IEnumerator CooldownCoroutine()
    {
        shootCooldownTime = equippedGunTypeInfo.ShotsInterval;
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
            Vector3 newHandPosition = shoulderPoint.position + (aimPointPosition - shoulderPoint.position).normalized;
            animator.SetIKPosition(AvatarIKGoal.RightHand, newHandPosition);
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
            lastEquippedGunType = equippedGunType;
            EquipGun(WeaponSystem.GunType.None);
        }
    }
}
