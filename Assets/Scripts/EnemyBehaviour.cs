using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float acceptanceRadius;
    public float detectionRange;

    [SerializeField] PlayerController _player;
    [SerializeField] ParticleSystem _particles;
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
        if (!_anim.GetBool("Attack") && _navMeshAgent.enabled)
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
        if (!_anim.GetBool("Hit"))
        {
            lifePoints -= 1;
            _particles.Play();
            if (lifePoints <= 0)
            {
                Death();
            }

            _anim.SetBool("Hit", true);
        }
    }

    private void Death()
    {
        _navMeshAgent.enabled = false;
        _particles = null;
        _anim.SetBool("Dead", true);
    }

    public void StopAnimation()
    {
        _anim.enabled = false;
    }
    
    public void HitEnd()
    {
        _anim.SetBool("Hit", false);
    }

}