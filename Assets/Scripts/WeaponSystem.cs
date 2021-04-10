using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { get; private set; }
    // Guns
    private Dictionary<GunType, GunTypeInfo> gunObjectsDict;
    private Dictionary<string, GameObject> gunPrefabsDict;
    // Shells
    private Dictionary<ShellType, ShellTypeInfo> shellObjectsDict;
    private Dictionary<string, GameObject> shellPrefabsDict;

    // Public getters
    public GunTypeInfo GetGunObject(GunType type) => gunObjectsDict[type];
    public GameObject GetGunPrefab(string prefabName) => gunPrefabsDict[prefabName];
    public ShellTypeInfo GetShellObject(ShellType type) => shellObjectsDict[type];
    public GameObject GetShellPrefab(string prefabName) => shellPrefabsDict[prefabName];
    
    public enum GunType
    {
        None,
        Pistol,
        Machinegun,
        Shotgun,
        Uzi,
        RPG
    }

    public enum ShellType
    {
        None,
        Bullet,
        Rocket
    }
    
    private void Awake()
    {
        Instance = this;
        InitializeData();
    }

    private void InitializeData()
    {
        InitializeGunData();
        InitializeShellData();
    }

    private void InitializeGunData()
    {
        gunObjectsDict = new Dictionary<GunType, GunTypeInfo>();
        GunTypeInfo[] gunObjects = Resources.LoadAll<GunTypeInfo>("GunObjects/");
        foreach (GunTypeInfo gunObject in gunObjects)
        {
            if (Enum.TryParse(gunObject.name, out GunType gunType))
            {
                gunObjectsDict.Add(gunType, gunObject);
            }
            else
            {
                Debug.LogWarning($"Cannot retrieve GunType for GunObject {gunObject.name}");
            }
        }

        gunPrefabsDict = new Dictionary<string, GameObject>();
        GameObject[] gunPrefs = Resources.LoadAll<GameObject>("GunPrefabs/");
        foreach (GameObject gunPref in gunPrefs)
        {
            gunPrefabsDict.Add(gunPref.name, gunPref);
        }
    }

    private void InitializeShellData()
    {
        shellObjectsDict = new Dictionary<ShellType, ShellTypeInfo>();
        ShellTypeInfo[] shellObjects = Resources.LoadAll<ShellTypeInfo>("ShellObjects/");
        foreach (ShellTypeInfo shellObject in shellObjects)
        {
            if (Enum.TryParse(shellObject.name, out ShellType shellType))
            {
                shellObjectsDict.Add(shellType, shellObject);
            }
            else
            {
                Debug.LogWarning($"Cannot retrieve ShellType for ShellObject {shellObject.name}");
            }
        }

        shellPrefabsDict = new Dictionary<string, GameObject>();
        GameObject[] shellPrefs = Resources.LoadAll<GameObject>("ShellPrefabs/");
        foreach (GameObject shellPref in shellPrefs)
        {
            shellPrefabsDict.Add(shellPref.name, shellPref);
        }
    }
}
