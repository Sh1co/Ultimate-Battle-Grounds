using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    [SerializeField] protected int _health = 100;
    public Transform Target;
    public Action TroopDied;
    public Action<Troop> TroopReady;
    public List<Troop> Enemies;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void Play()
    {
        _agent.speed = _originalSpeed;
        // TODO : enable attacking
    }

    public void Pause()
    {
        _agent.speed = 0;
        //TODO : disable attacking
    }
    
    
    public void FindNewTarget()
    {
        Troop target = null;
        float minDistance = float.MaxValue;
        foreach (var troop in Enemies)
        {
            if (troop == null) continue;

            Vector3 distance = troop.transform.position - transform.position;
            if (distance.magnitude < minDistance)
            {
                minDistance = distance.magnitude;
                target = troop;
            }
        }
        if (target != null) SetTarget(target);
    }

    public void SetTarget(Troop target)
    {
        target.TroopDied += TargetDied;
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

            if (_agent.pathStatus == NavMeshPathStatus.PathComplete && !_signaledReady)
            {
                TroopReady?.Invoke(this);
                _signaledReady = true;
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
    
    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _originalSpeed = _agent.speed;
    }

    
    protected NavMeshAgent _agent;
    private bool _lockedOnTarget;
    private bool _signaledReady;
    private float _originalSpeed;
}