using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private List<Enemy> _enemies = new List<Enemy>();
    private Enemy CurrentEnemyTarget;
    private float fireCooldown;

    private void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsTarget();

        if (fireCooldown <= 0f && CurrentEnemyTarget != null)
        {
            FireAtTarget();
            fireCooldown = 1f / fireRate; // Reset cooldown
        }

        fireCooldown -= Time.deltaTime;
    }

    private void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            CurrentEnemyTarget = null;
            return;
        }

        CurrentEnemyTarget = _enemies[0]; // Target first enemy in list
    }

    private void RotateTowardsTarget()
    {
        if (CurrentEnemyTarget == null)
            return;

        Vector3 direction = CurrentEnemyTarget.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FireAtTarget()
    {
        if (bulletPrefab == null || firePoint == null || CurrentEnemyTarget == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Projectile bulletScript = bullet.GetComponent<Projectile>();

        if (bulletScript != null)
            bulletScript.SetTarget(CurrentEnemyTarget);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            if (newEnemy != null && !_enemies.Contains(newEnemy))
                _enemies.Add(newEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _enemies.Contains(enemy))
                _enemies.Remove(enemy);
        }
    }
}