using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform _targetTransform;

    [Header("Attributes")]
    [SerializeField] float range = 15f;
    [SerializeField] float fireRate = 1f;
    float _fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    [SerializeField] string targetTag = "Card";

    [SerializeField] Transform partToRotate;
    [SerializeField] float turnSpeed = 10f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootingPoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets)    
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null && shortestDistance <= range)
        {
            _targetTransform = nearestTarget.transform;
        }
        else
        {
            _targetTransform = null;
        }
    }

    void Update()
    {
        if (_targetTransform == null)
            return;

        // Target lock on
        Vector3 dir = _targetTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation);

        if (_fireCountdown <= 0)
        {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(_targetTransform);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
