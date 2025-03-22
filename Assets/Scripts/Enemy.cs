using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float reachThreshold = 0.2f;
    [SerializeField] private int health = 3; // Added health variable

    private Waypoint _waypoint;
    private int _currentWaypointIndex;
    private bool _isActive;

    public void Initialize(Waypoint waypoint)
    {
        _waypoint = waypoint;
        _currentWaypointIndex = 0;
        _isActive = true;
        transform.position = _waypoint.GetWaypointPosition(_currentWaypointIndex);
    }

    private void Update()
    {
        if (!_isActive || _waypoint == null) return;

        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        if (_currentWaypointIndex >= _waypoint.Points.Length) 
        {
            Recycle();
            return;
        }

        Vector3 targetPosition = _waypoint.GetWaypointPosition(_currentWaypointIndex);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < reachThreshold)
        {
            _currentWaypointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isActive = false;
        ObjectPooler.ReturnToPool(gameObject);
    }

    private void Recycle()
    {
        _isActive = false;
        ObjectPooler.ReturnToPool(gameObject);
    }
}