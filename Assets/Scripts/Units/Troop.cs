﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    [SerializeField] protected int _health = 100;
    public Transform Target;
    public Action TroopDied;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void SetTarget(Troop target)
    {
        StartCoroutine(FollowTarget(1f, target.transform));
    }
    
    protected IEnumerator FollowTarget(float range, Transform target) {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        _lockedOnTarget = true;
        while (_lockedOnTarget) {
            if (Vector3.SqrMagnitude (previousTargetPosition - target.position) > range) {
                _agent.SetDestination (target.position);
                previousTargetPosition = target.position;
            }
            yield return new WaitForSeconds (0.7f);
        }
        yield return null;
    }
    
    private void Die()
    {
        TroopDied?.Invoke();
        Destroy(gameObject);
    }

    private void TargetDied()
    {
        _lockedOnTarget = false;
        FindNewTarget();
    }

    private void FindNewTarget()
    {
        throw new NotImplementedException();
    }

    protected Troop _target;
    protected NavMeshAgent _agent;
    private bool _lockedOnTarget;
}