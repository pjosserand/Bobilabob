using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public float lifePoints;
    public float maxLifePoints;

    [Range(1f, 50f)] [SerializeField] private float _rayMaxDistance = 20f;

    [SerializeField] LayerMask _groundLayer;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] Material _bloodScreen;
    [SerializeField] GameObject _Shield;

    public GameManager gmInstance;
    private Camera _mainCamera;
    private NavMeshAgent _agent;
    public Animator _animator;
    public EnemyBehaviour _enemy;
    private bool isAttacking;
    private bool shieldActived;
    private int damage = 1;
    private Vector3 _destinationPres;

    public CinemachineImpulseSource _screenShake;

    void Start()
    {
        _mainCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        maxLifePoints = 4;
        lifePoints = maxLifePoints;
        _bloodScreen.SetFloat("_Power", 5.0f);
        gmInstance = GameManager.Instance;
        _Shield.SetActive(false);
        shieldActived = false;
    }

    private void Update()
    {
        _animator.SetFloat("Velocity", Math.Abs(_agent.velocity.x + _agent.velocity.z));
		Shader.SetGlobalVector("_PlayerPos",transform.position);
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

    void OnLeftClick(InputValue prminput)
    {
        //Debug.Log("Left clicked");
        attack();
    }

    void OnSpace(InputValue prminput)
    {
        if (prminput.isPressed)
        {
           gmInstance.Pause();
        }
    }
    
    void attack()
    {
        if (_agent.destination != null)
        {
            _destinationPres = _agent.destination;
        }
        if (_enemy != null)
        {
            _enemy.TakeDamage();
        }
        _agent.SetDestination(transform.position);
        //Call animations
         isAttacking=true;
        _animator.SetBool("isAttacking", isAttacking);
        Invoke(nameof(stopAttack),1f);
    }

    public void stopAttack()
    {
       //Debug.Log("Stop Attack !!!");
       isAttacking=false;
       if (_destinationPres != null)
       {
           _agent.destination = _destinationPres;
       }
       _animator.SetBool("isAttacking", isAttacking);
    }

    void move()
    {
        if(!isAttacking)
        {
            Ray cameraRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(cameraRay, out hitInfo, _rayMaxDistance, _groundLayer.value))
                {
                    _agent.SetDestination(hitInfo.point);
                }
        }
    }


    public void Damage()
    {
        if (!shieldActived)
        {
            _screenShake.GenerateImpulse();
            Debug.Log("hit");
            lifePoints -= 1;
            _particles.Play();
            UpdateBloodScreen();
            if (lifePoints <= 0)
            {
                Death();
            }
            else
            {
                _animator.SetBool("Hit", true);
            }
        }
    }

    void UpdateLife(int addToLife)
    {
        lifePoints += addToLife;
        if (lifePoints > maxLifePoints)
        {
            lifePoints = maxLifePoints;
        }
        else if (lifePoints <= 0)
        {
            Death();
        }
        UpdateBloodScreen();
    }

    IEnumerator RemoveShield()
    {
        shieldActived = true;
        _Shield.SetActive(true);
        yield return new WaitForSeconds(5);
        shieldActived = false;
        _Shield.SetActive(false);
    }


    public void UpdateBloodScreen()
    {
        switch (lifePoints){
            case 3:
                _bloodScreen.SetFloat("_Power", 2.0f);
                break;
            case 2:
                _bloodScreen.SetFloat("_Power", 1.0f);
                break;
            case 1:
                _bloodScreen.SetFloat("_Power", 0.5f);
                break;
            default :
                _bloodScreen.SetFloat("_Power", 5f);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            UpdateLife(2);
            other.gameObject.SetActive(false);
            StartCoroutine(Health());

        }
        else if (other.CompareTag("Shield"))
        {
            StartCoroutine(RemoveShield());
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Goal"))
        {
            gmInstance.Win();
        }
    }

    IEnumerator Health()
    {
        Shader.SetGlobalFloat("_ApplePickedUp",1);
        yield return new WaitForSeconds(1f);
        Shader.SetGlobalFloat("_ApplePickedUp",0);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (!isAttacking) return;
            other.gameObject.GetComponent<DoorScript>().TakeDamage(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            if (!isAttacking) return;
            other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage();
        }
    }

    void Death()
    {
        Debug.Log("dead");
        _animator.SetBool("Dead", true);
        _agent.SetDestination(transform.position);
        gmInstance.GameOver();
        //DeathStuff
    }

    public void HitEnd()
    {
        _animator.SetBool("Hit", false);
    }
}
