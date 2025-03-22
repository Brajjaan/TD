using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private GameObject testGO;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    private float _spawnTimer;
    private int _enemiesSpawned;

    private ObjectPooler _pooler;

    private void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = delayBtwSpawns;
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
    
        // Force Z = 0 when spawning
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0);
        newInstance.transform.position = spawnPos;

        newInstance.SetActive(true);

        Enemy enemyScript = newInstance.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.Initialize(FindObjectOfType<Waypoint>());
        }

        Debug.Log($"Spawned at: {newInstance.transform.position}");
    }
}

