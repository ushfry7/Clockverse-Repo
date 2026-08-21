using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseEnemyAI : MonoBehaviour
{
    public Image _healthBar;
    NavMeshAgent _agent;
    GameObject _target;
    Animator _animator;
    float _currentHealth, _valueChangeAmount = 0.5f, _speed = 2.5f;
    [SerializeField] float _maxHealth = 100f, _damage = 10f, _hitMaxDistance = 2.5f;
    RaycastHit _hit; 
    GameUIManager _gameUIManager;
    bool _haveTakenDamage = false;
    int _newPlayerStrike;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        ComponentAssignment();
    }

    void ComponentAssignment()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _agent.stoppingDistance = 2f;
        _animator = GetComponent<Animator>();
        _target = FindFirstObjectByType<BasePlayerController>().gameObject;
        _gameUIManager = FindFirstObjectByType<GameUIManager>();
        FaceTarget();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) > _agent.stoppingDistance && _currentHealth > 0)
        {
            _agent.SetDestination(_target.transform.position);
            _animator.SetBool("Run", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Block", false);
        }
        else
        {
            _animator.SetBool("Run", false);
            if (_agent.speed > 0)
            {
                transform.LookAt(_target.transform.position);
            }
            if (_target.GetComponent<BasePlayerController>()._attacking && _haveTakenDamage)
            {
                _animator.SetBool("Block", true);
                _animator.SetBool("Attack", false);
            }
            else
            {
                _animator.SetBool("Attack", true);
                _animator.SetBool("Block", false);
            }
            if (!_target.GetComponent<BasePlayerController>()._attacking && _target.GetComponent<BasePlayerController>()._comboCount == _newPlayerStrike)
            {
                _haveTakenDamage = false;
            }
        }
        _healthBar.fillAmount = Mathf.MoveTowards(_healthBar.fillAmount, _currentHealth / _maxHealth, _valueChangeAmount * Time.deltaTime);
    }

    public void TakeDamage(float value)
    {
        if (_target.GetComponent<BasePlayerController>()._comboCount > 1 || !_animator.GetBool("Block"))
        {
            _currentHealth -= value;
            _newPlayerStrike = _target.GetComponent<BasePlayerController>()._comboCount + 1;
            if (_newPlayerStrike == 3) 
            {
                _newPlayerStrike = 0;
            }
        }
        if (_currentHealth <= 0)
        {
            _animator.SetBool("Dead", true);
        }
        FindFirstObjectByType<TempoSystem>()._currentRecharge += Mathf.RoundToInt(value);
        _gameUIManager.RechargeUpdate();
        _haveTakenDamage = true;
    }

    void Strike()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, _hitMaxDistance))
        {
            if (_hit.collider.TryGetComponent<BasePlayerController>(out BasePlayerController _player))
            {
                _player.TakeDamage(_damage);
            }
        }
    }

    void Dead()
    {
        FindFirstObjectByType<GameUIManager>().WaveSpawn();
        gameObject.SetActive(false);
    }

    public void PermanentSlow(float _slowDown)
    {
        _agent.speed -= (_agent.speed * _slowDown / 100);
        _animator.speed -= (_animator.speed * _slowDown / 100);
    }

    public void SlowDown(float _duration, float _slowDown, float _delay = 0, bool _continous = false, float _activationDelay = 0)
    {
        StartCoroutine(SlowDownTempo(_duration, _slowDown, _delay, _continous, _activationDelay));
    }

    IEnumerator SlowDownTempo(float _duration, float _slowDown, float _delay, bool _continous, float _activationDelay)
    {
        yield return new WaitForSeconds(_activationDelay);
        _agent.speed -= (_agent.speed * _slowDown / 100);
        _animator.speed = (_animator.speed * _slowDown / 100);
        if (_continous)
        {
            yield return new WaitUntil(_gameUIManager._player.HaveForce);
        }
        else
        {
            yield return new WaitForSeconds(_duration);
        }
        yield return new WaitForSeconds(_delay);
        _agent.speed = _speed;
        _animator.speed = 1f;
    }

    public void Freeze(float value)
    {
        StartCoroutine(FreezeTempo(value));
    }

    IEnumerator FreezeTempo(float value)
    {
        _agent.speed = 0f;
        _agent.isStopped = true;
        _animator.speed = 0f;
        yield return new WaitForSeconds(value);
        _agent.speed = _speed;
        _animator.speed = 1f;
        _agent.isStopped = false;
    }

    void FaceTarget()
    {
        Vector3 direction = _target.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction);
    }
    public void Normal()
    {
        _agent.speed = _speed;
        _animator.speed = 1f;
    }
}
