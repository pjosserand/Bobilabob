using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public int lifePoints;

    [Range(1f, 50f)] [SerializeField] private float _rayMaxDistance = 20f;

    [SerializeField] LayerMask _groundLayer;

    private Camera _mainCamera;
    private NavMeshAgent _agent;
    public Animator _animator;

    void Start()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _animator.SetFloat("Velocity", Math.Abs(_agent.velocity.x + _agent.velocity.z));
		Shader.SetGlobalVector("worldSpace_PlayerPos",transform.position);
    }

    void OnRightClick(InputValue prminput)
    {
        if (prminput.isPressed)
        {
           InvokeRepeating(nameof(move), 0f, 0.2f );
        }
        else
        {
            CancelInvoke();
        }
    }

    void move()
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