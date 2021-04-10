using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerSettings", order = 2)]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float movementSpeed;

    public float MovementSpeed => movementSpeed;
}
