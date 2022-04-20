using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionPlayer : MonoBehaviour
{
    
    
    [SerializeField] EnemyBehaviour _enemyBehaviour;
    [SerializeField] PlayerController _player;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == _player)
        {
            _enemyBehaviour.Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == _player)
        {
            _enemyBehaviour.StopAttack();
        }
    }
}
