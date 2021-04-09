using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GunInfo", order = 0)]
public class GunObject : ScriptableObject
{
    [SerializeField] private string prefabName;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float bulletSpread;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shotsInterval;

    public string PrefabName => prefabName;
    public int BulletsPerShot => bulletsPerShot;
    public float BulletSpread => bulletSpread;
    public float BulletSpeed => bulletSpeed;
    public float ShotsInterval => shotsInterval;
}
