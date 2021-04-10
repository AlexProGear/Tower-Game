using System;
using ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;
    
    private float speed;
    public float collisionForce { get; private set; }

    private const float LifeTime = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, LifeTime);
    }

    public void Setup(ShellTypeInfo info)
    {
        speed = info.ShellSpeed;
        collisionForce = info.CollisionForce;
    }
    
    private void FixedUpdate()
    {
        rb.position += transform.forward * (Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.isTrigger)
            return;
        
        Destroy(gameObject);
    }
}
