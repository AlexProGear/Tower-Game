using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int enemyLimit;
    [SerializeField] private float spawnRadius;

    public static EnemySpawner Instance { get; private set; }

    private HashSet<GameObject> activeEnemies;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        activeEnemies = new HashSet<GameObject>();
        AddEnemies();
    }

    public void RemoveDead(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        AddEnemies();
    }

    private void AddEnemies()
    {
        while (activeEnemies.Count < enemyLimit)
        {
            GameObject newEnemy = SpawnEnemy();
            activeEnemies.Add(newEnemy);
        }
    }

    private GameObject SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        Vector2 positionOnPlane = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(positionOnPlane.x, 0.5f, positionOnPlane.y);
        newEnemy.GetComponent<NavMeshAgent>().Warp(spawnPos);
        return newEnemy;
    }
}
