using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private EnemyMovement enemyMovement;
    private bool ragdollStatus = false;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (!ragdollStatus)
            {
                ragdollStatus = true;
                EnableRagdoll();
                EnemySpawner.Instance.RemoveDead(this.gameObject);
                Destroy(gameObject, 10f);
            }
            PushRagdoll(other.GetComponent<BulletScript>());
        }
    }

    private void PushRagdoll(BulletScript bullet)
    {
        float bulletForce = bullet.collisionForce;
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.AddExplosionForce(bulletForce, bullet.transform.position, 1f, bulletForce/10f, ForceMode.Impulse);
        }
    }

    private void EnableRagdoll(bool enable = true)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !enable;
        }
        EnableComponents(false);
    }

    private void EnableComponents(bool enable)
    {
        animator.enabled = enable;
        if (!enable) navMeshAgent.ResetPath();
        navMeshAgent.enabled = enable;
        enemyMovement.enabled = enable;
        gameObject.layer = enable ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Ignore Raycast");
    }
}
