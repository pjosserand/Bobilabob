using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class DoorScript : MonoBehaviour
{
    public int life;
    private Material _doorMat;
    private Material _glassMat;
    private NavMeshObstacle _doorObstacle;
    public float coolDown;
    public float maxCoolDown;

    public float dissolveTime;
    public float maxDissolveTime;
    public string startDissolveName;
    public string updateDissolveName;
    // Start is called before the first frame update
    void Start()
    {
        life = 2;
        coolDown = 0.0f;
        maxCoolDown = 1.0f;
        maxDissolveTime = 1.0f;
        dissolveTime = maxDissolveTime+0.1f;
        //startDissolveName = "_HasToDissolve";
        updateDissolveName = "_DissolveAmmount";
        _doorObstacle = transform.GetChild(1).gameObject.GetComponent<NavMeshObstacle>();
        _doorMat = transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        _glassMat = transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material;
        _doorMat.SetFloat(updateDissolveName,0);
        _glassMat.SetFloat(updateDissolveName,0);

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
        DissolveTimer();
    }

    void DissolveTimer()
    {
        if (dissolveTime > maxDissolveTime) return;
        dissolveTime += Time.deltaTime;
        _doorMat.SetFloat(updateDissolveName,dissolveTime);
        _glassMat.SetFloat(updateDissolveName,dissolveTime);
        if (dissolveTime > maxDissolveTime)
        {
            dissolveTime = maxDissolveTime;
            DesactivateDoor();
        }
    }

    void DesactivateDoor()
    {
        _doorMat.SetFloat(updateDissolveName, maxDissolveTime);
        _glassMat.SetFloat(updateDissolveName,dissolveTime);
        _doorObstacle.enabled = false;
    }
    
    public void TakeDamage(int damage)
    {
        if (life <= 0) return;
        if (coolDown > 0) return;
        UpdateLife(-damage);
        coolDown = maxCoolDown;
        //LaunchParticules
    }
    
    void UpdateLife(int addToLife)
    {
        if (life <= 0) return;
        life += addToLife;
        if (life <= 0)
        {
            dissolveTime = 0;
        }
    }
}
