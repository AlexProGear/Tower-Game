using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GunTypeInfo", order = 0)]
public class GunTypeInfo : ScriptableObject
{
    [SerializeField] private string prefabName;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float bulletSpread;
    [SerializeField] private WeaponSystem.ShellType shellType;
    [SerializeField] private float shotsInterval;

    public string PrefabName => prefabName;
    public int BulletsPerShot => bulletsPerShot;
    public float BulletSpread => bulletSpread;
    public WeaponSystem.ShellType ShellType=> shellType;
    public float ShotsInterval => shotsInterval;
}
