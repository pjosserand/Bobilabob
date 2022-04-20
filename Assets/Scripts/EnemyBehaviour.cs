using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float acceptanceRadius;
    public float detectionRange;

    [SerializeField] PlayerController _player;
    private NavMeshAgent _navMeshAgent;
    private Animator _anim;
    private int lifePoints;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        lifePoints = 2;
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Speed", Math.Abs(_navMeshAgent.velocity.x + _navMeshAgent.velocity.z));
        Ray ray = new Ray(transform.position, _player.transform.position - transform.position);
        RaycastHit hitInfo;
        if (!_anim.GetBool("Attack"))
        {
            if (Physics.Raycast(ray, out hitInfo, detectionRange) && hitInfo.collider.CompareTag("Player"))
            {

                if ((_player.transform.position - transform.position).magnitude >= acceptanceRadius)
                {
                    _navMeshAgent.SetDestination(_player.transform.position);

                }
                else
                {
                    _navMeshAgent.SetDestination(transform.position);
                }
            }
            else
            {
                _navMeshAgent.SetDestination(transform.position);
            }
        }
    }
    
    public void Attack()
    {
        _anim.SetBool("Attack", true);
    }
    
    public void StopAttack()
    {
        _anim.SetBool("Attack", false);
    }

    public void MakeDamage()
    {
        _player.Damage();
    }
    public void TakeDamage()
    {
        lifePoints -= 1;
        if (lifePoints <= 0)
        {
            Death();
        }
        _anim.SetBool("Hit", true);
    }

    private void Death()
    {
        _anim.SetBool("Dead", true);
    }

}