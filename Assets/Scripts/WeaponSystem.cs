using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public static WeaponSystem Instance { get; private set; }
    public Dictionary<GunType, GunObject> GunObjectsDict { get; private set; }
    public Dictionary<string, GameObject> GunPrefabsDict { get; private set; }
    
    public enum GunType
    {
        None,
        Pistol,
        Machinegun,
        Shotgun,
        Uzi,
        RPG
    }
    
    private void Awake()
    {
        Instance = this;
        InitializeData();
    }

    private void InitializeData()
    {
        GunObjectsDict = new Dictionary<GunType, GunObject>();
        GunObject[] gunObjects = Resources.LoadAll<GunObject>("GunObjects/");
        foreach (GunObject gunObject in gunObjects)
        {
            if (Enum.TryParse(gunObject.name, out GunType gunType))
            {
                GunObjectsDict.Add(gunType, gunObject);
            }
            else
            {
                Debug.LogWarning($"Cannot retrieve GunType for GunObject {gunObject.name}");
            }
        }

        GunPrefabsDict = new Dictionary<string, GameObject>();
        GameObject[] gunPrefs = Resources.LoadAll<GameObject>("GunPrefabs/");
        foreach (GameObject gunPref in gunPrefs)
        {
            GunPrefabsDict.Add(gunPref.name, gunPref);
        }
    }
}
