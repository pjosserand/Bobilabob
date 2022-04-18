using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class DoorScript : MonoBehaviour
{
    public int life;
    public NavMeshObstacle door;
    public float coolDown;
    public float maxCoolDown;
    // Start is called before the first frame update
    void Start()
    {
        life = 2;
        maxCoolDown = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown <= 0) return;
        coolDown -= Time.deltaTime;
        if (coolDown <= 0)
        {
            coolDown = 0;
        }
    }

    public void takeDamage(int damage)
    {
        Debug.Log("Player hit the door");
        if (coolDown > 0) return;
        UpdateLife(-damage);
        coolDown = maxCoolDown;
        //LaunchParticules
    }
    
    void UpdateLife(int addToLife)
    {
        life += addToLife;
        if (life <= 0)
        {
            //Debug.Log("Door is opened");
            door.gameObject.SetActive(false);
            door.enabled = false;
        }
    }
}
