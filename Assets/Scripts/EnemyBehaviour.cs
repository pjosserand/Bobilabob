using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float acceptanceRadius;
    public float detectionRange;

    private GameObject _player;
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, _player.transform.position - transform.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, detectionRange))
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

    void Attack()
    {
        
    }
}