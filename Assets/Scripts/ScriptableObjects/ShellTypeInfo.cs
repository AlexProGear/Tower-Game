using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ShellTypeInfo", order = 1)]
public class ShellTypeInfo : ScriptableObject
{
    [SerializeField] private string prefabName;
    [SerializeField] private float shellSpeed;
    [SerializeField] private float collisionForce;
    
    public string PrefabName => prefabName;
    public float ShellSpeed => shellSpeed;
    public float CollisionForce => collisionForce;
}