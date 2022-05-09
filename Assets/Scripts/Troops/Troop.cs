using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    [SerializeField] private TroopStates _startingState = TroopStates.Idle;
    public int Health = 100;
    public int AttackValue = 20;
    [SerializeField] protected float _attackRange = 2.0f;
    [SerializeField] protected float _attackInterval = 2.0f;
    [SerializeField] protected float _searchRadius = 100.0f;
    [SerializeField] private LayerMask _searchMask;


    public Action<Troop> TroopDied;
    public int Team { get; set; }
    [SerializeField] private float _obstacleToAgentSwitchDelay = 0.15f;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Play()
    {
        FindNewTarget();
    }

    public void Pause()
    {
        StartCoroutine(SwitchState(TroopStates.Idle));
    }

    public void GoToPosition(Vector3 position)
    {
        if(_state!= TroopStates.UnderCommand)_stateBeforeCommand = _state;
        StartCoroutine(FollowCommand(position));
    }
    
    public void FindNewTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _searchRadius,_searchMask);
        Troop target = null;
        float minDistance = float.MaxValue;
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.GetComponent<Troop>().Team == Team)continue;
            Vector3 distance = hitCollider.transform.position - transform.position;
            if (distance.magnitude < minDistance)
            {
                minDistance = distance.magnitude;
                target = hitCollider.GetComponent<Troop>();
            }
        }

        if (target != null) SetTarget(target);
        else StartCoroutine(SwitchState(TroopStates.Idle));

    }

    private void SetTarget(Troop target)
    {
        _target = target;
        StartCoroutine(FollowTarget(target.transform));
    }

    private IEnumerator FollowTarget(Transform target)
    {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        yield return StartCoroutine(SwitchState(TroopStates.Attacking));
        while (_state == TroopStates.Attacking)
        {
            if (target == null)
            {
                _obstacle.enabled = false;
                yield return new WaitForSeconds(_obstacleToAgentSwitchDelay);
                _agent.enabled = true;
                _agent.ResetPath();
                FindNewTarget();
                break;
            }
            if (Vector3.Distance(transform.position, target.position) >= _attackRange)
            {
                if(_obstacle.enabled || !_agent.enabled)
                {
                    _obstacle.enabled = false;
                    yield return new WaitForSeconds(_obstacleToAgentSwitchDelay);
                    _agent.enabled = true;
                }
                if (target == null) continue;
                if (_agent.isStopped) _agent.isStopped = false;
                if (Vector3.Distance(previousTargetPosition, target.position) >= 1f)
                {
                    _agent.SetDestination(target.position);
                    previousTargetPosition = target.position;
                }
            }
            else
            {
                _agent.enabled = false;
                _obstacle.enabled = true;
            }

            yield return new WaitForSeconds(0.7f);
        }
    }

    private IEnumerator FollowCommand(Vector3 position)
    {
        yield return StartCoroutine(SwitchState(TroopStates.UnderCommand));
        _agent.SetDestination(position);
    }

    private IEnumerator SwitchState(TroopStates state)
    {
        switch (state)
        {
            case TroopStates.Idle:
                if(_agent.hasPath) _agent.ResetPath();
                _agent.enabled = false;
                _obstacle.enabled = true;
                _state = state;
                break;
            case TroopStates.Attacking:
            case TroopStates.UnderCommand:
                _obstacle.enabled = false;
                yield return new WaitForSeconds(_obstacleToAgentSwitchDelay);
                _agent.enabled = true;
                _state = state;
                break;
            case TroopStates.StandGround:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case TroopStates.Idle:
                break;
            case TroopStates.Attacking:
                Attack();
                break;
            case TroopStates.UnderCommand:
                if (_agent.remainingDistance < 0.1f && _agent.hasPath)
                {
                    if (_stateBeforeCommand == TroopStates.Attacking) FindNewTarget();
                    else StartCoroutine(SwitchState(_stateBeforeCommand));
                }
                break;
            case TroopStates.StandGround:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Attack()
    {
        if (_target == null) return;
        if (_attackWait < _attackInterval) _attackWait += Time.deltaTime;
        if (!(Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)) return;
        if (_attackWait < _attackInterval) return;
        _target.TakeDamage(AttackValue);
        _attackWait = 0;
    }

    private void Die()
    {
        TroopDied?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(Vector3.zero);
        _obstacle = GetComponent<NavMeshObstacle>();
        StartCoroutine(SwitchState(_startingState));
    }


    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;
    private Troop _target;
    private bool _lockedOnTarget;
    private float _attackWait;
    private TroopStates _state;
    private TroopStates _stateBeforeCommand;
}

