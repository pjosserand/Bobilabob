using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{

    public int lifePoints;
    
    [Range(1f, 50f)]
    [SerializeField] private float _rayMaxDistance = 20f;

    [SerializeField] LayerMask _groundLayer;

    private Camera _mainCamera;
    private NavMeshAgent _agent;

    void Start()
    {
        _mainCamera = Camera.main;    
        _agent = GetComponent<NavMeshAgent>();
    }

    void OnRightClick()
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, _rayMaxDistance, _groundLayer.value))
        {
            _agent.SetDestination(hitInfo.point);
        }
    }


    void Damage(int prmDamageValue)
    {
        lifePoints -= prmDamageValue;

        if (lifePoints <= 0)
        {
            Death();
        }
        
    }

    void Death()
    {
        //DeathStuff
    }
    
}
