using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed;

    private void Start()
    {
        UpdatePositionForNewTarget();
    }

    private void UpdatePositionForNewTarget()
    {
        transform.position = targetTransform.position + offset;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position + offset, Time.deltaTime * followSpeed);
    }
}
