using System;
using ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BulletScript : MonoBehaviour
{
    private Rigidbody rb;
    
    private float speed;
    private float collisionForce;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        Destroy(gameObject);
    }
}
