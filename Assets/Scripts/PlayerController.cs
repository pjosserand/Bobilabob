using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
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

   
    void Update()
    {
        //Shader.SetGlobalVector("WS_PlayerPosition", transform.position);

        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(cameraRay, out hitInfo, _rayMaxDistance, _groundLayer.value))
        {
            _agent.SetDestination(hitInfo.point);
        }
    }
}
