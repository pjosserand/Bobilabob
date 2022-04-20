using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionEnemy : MonoBehaviour
{
     
    [SerializeField] PlayerController _playerController;
    
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // other.gameObject
            _playerController._enemy = other.gameObject.GetComponent<EnemyBehaviour>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playerController._enemy == other.gameObject)
        {
            _playerController._enemy = null;
        }
    }
}
