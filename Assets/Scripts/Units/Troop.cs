using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    [SerializeField] protected int _health = 100;
    [SerializeField] protected float _attackRange = 2.0f;
    [SerializeField] protected float _attackInterval = 2.0f;
    [SerializeField] protected int _attackValue = 20;
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
        _agent.isStopped = false;
        _paused = false;
        _attacking = true;
    }

    public void Pause()
    {
        _agent.isStopped = true;
        _paused = true;
        _attacking = false;
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
        _target = target;
        StartCoroutine(FollowTarget(target.transform));
    }

    protected IEnumerator FollowTarget(Transform target)
    {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        _lockedOnTarget = true;
        while (_lockedOnTarget)
        {
            if (target == null)
            {
                _agent.ResetPath();
                FindNewTarget();
                break;
            }

            if (Vector3.SqrMagnitude(transform.position - target.position) >= _attackRange)
            {
                if (_agent.isStopped && !_paused) _agent.isStopped = false;
                if (Vector3.SqrMagnitude(previousTargetPosition - target.position) >= 0.5f)
                {
                    _agent.SetDestination(target.position);
                    previousTargetPosition = target.position;
                }
            }
            else
            {
                _agent.isStopped = true;
            }

            if (_agent.pathStatus == NavMeshPathStatus.PathComplete && !_signaledReady)
            {
                TroopReady?.Invoke(this);
                _signaledReady = true;
            }

            yield return new WaitForSeconds(0.7f);
        }

        yield return null;
    }

    private void Update()
    {
        if (_attackWait < _attackInterval) _attackWait += Time.deltaTime;
        if (_target == null || !_attacking) return;
        if (Vector3.SqrMagnitude(transform.position - _target.transform.position) <= _attackRange)
        {
            if (_attackWait < _attackInterval) return;
            Attack();
            _attackWait = 0;
        }
    }

    private void Attack()
    {
        _target.TakeDamage(_attackValue);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    protected NavMeshAgent _agent;
    public Troop _target;
    private bool _lockedOnTarget;
    private bool _signaledReady;
    private float _attackWait;
    private bool _attacking = true;
    private bool _paused = false;
}