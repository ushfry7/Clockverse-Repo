using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseEnemyAI : MonoBehaviour
{
    public enum TargetType
    {
        player,
        enemy
    }
    public TargetType _targetType = TargetType.player;
    public Image _healthBar;
    NavMeshAgent _agent;
    GameObject _target;
    Animator _animator;
    float _currentHealth, _valueChangeAmount = 0.5f, _speed = 2.5f;
    [SerializeField] float _maxHealth = 100f, _damage = 10f, _hitMaxDistance = 2.5f;
    RaycastHit _hit; 
    GameUIManager _gameUIManager;
    bool _haveTakenDamage = false, _notMove = false;
    public bool _attacking = false;
    int _newPlayerStrike;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        ComponentAssignment();
    }

    void ComponentAssignment()
    {
                _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.speed = _speed;
        _agent.stoppingDistance = 2f;
        _gameUIManager = FindFirstObjectByType<GameUIManager>();
    }

    public void TargetTypeEnemy()
    {
        _targetType = TargetType.enemy;
    }
    void Update()
    {
        if (!_notMove)
        {           
            SearchForTarget();
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.transform.position) > _agent.stoppingDistance && _currentHealth > 0)
                {
                    _agent.SetDestination(_target.transform.position);
                    if (_targetType == TargetType.enemy)
                    {
                        _animator.SetInteger("Force", 1);
                    }
                    else
                    {
                        _animator.SetBool("Run", true);
                    }
                    _animator.SetBool("Attack", false);
                    _animator.SetBool("Block", false);
                }
                else
                {
                    if (_targetType == TargetType.enemy)
                    {
                        _animator.SetInteger("Force", 0);
                    }
                    else
                    {
                        _animator.SetBool("Run", false);
                    }
                    if (_currentHealth > 0 && _animator.speed > 0)
                    {
                        transform.LookAt(_target.transform.position);
                    }
                    if (_targetType == TargetType.player)
                    {
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
                    else
                    {
                        _animator.SetBool("Attack", true);
                        _animator.SetBool("Block", false);
                    }
                }
            }
            else
            {
                if (_targetType == TargetType.enemy)
                {
                    _animator.SetInteger("Force", 0);
                }
                else
                {
                    _animator.SetBool("Run", false);
                }
                _animator.SetBool("Attack", false);
                _animator.SetBool("Block", false);
            }
        }
        if (_healthBar != null)
        {
            _healthBar.fillAmount = Mathf.MoveTowards(_healthBar.fillAmount, _currentHealth / _maxHealth, _valueChangeAmount * Time.deltaTime);
        }
    }
    void AttackReset()
    {

    }
    void SearchForTarget()
    {
        if (_targetType == TargetType.player)
        {
            foreach (GameObject _player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (!_player.GetComponent<BaseEnemyAI>())
                {
                    if (_player.GetComponent<BasePlayerController>()._currentHealth > 0)
                    {
                        if (_target == null)
                        {
                            _target = _player.gameObject;
                        }
                        if (Vector3.Distance(_target.transform.position, transform.position) > Vector3.Distance(_player.transform.position, transform.position))
                        {
                            _target = _player.gameObject;
                        }
                    }
                }
                else
                {
                    if (_player.GetComponent<BaseEnemyAI>()._currentHealth > 0)
                    {
                        if (_target == null)
                        {
                            _target = _player.gameObject;
                        }
                        if (Vector3.Distance(_target.transform.position, transform.position) > Vector3.Distance(_player.transform.position, transform.position))
                        {
                            _target = _player.gameObject;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (BaseEnemyAI _enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                if (_enemy.gameObject != gameObject && _enemy._targetType != TargetType.enemy)
                {
                    if (_target == null)
                    {
                        _target = _enemy.gameObject;
                    }
                    if (Vector3.Distance(_target.transform.position, transform.position) > Vector3.Distance(_enemy.transform.position, transform.position))
                    {
                        _target = _enemy.gameObject;
                    }
                }
            }
        }
    }

    public void TakeDamage(float value, GameObject _dealer)
    {
        if(FindFirstObjectByType<StarTempo>().enabled)
        {
            FindFirstObjectByType<StarTempo>().ChargeIncrease(1);
        }
        if (_targetType == TargetType.player)
        {
            if (!_dealer.GetComponent<BaseEnemyAI>())
            {
                if (_dealer.GetComponent<BasePlayerController>())
                {
                    if (_dealer.GetComponent<BasePlayerController>()._comboCount > 1 || !_animator.GetBool("Block"))
                    {
                        _currentHealth -= value;
                        _newPlayerStrike = _dealer.GetComponent<BasePlayerController>()._comboCount + 1;
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
                else
                {
                    _currentHealth -= value;
                }
            }
            else
            {
                _currentHealth -= value;
                if (_targetType == TargetType.player)
                {
                    if (_currentHealth <= 0)
                    {
                        _animator.SetBool("Dead", true);
                    }
                }
            }
        }
    }

    public bool IsHealthy()
    {
        return _currentHealth > _maxHealth * 0.25f;
    }

    void Strike()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _hit, _hitMaxDistance))
        {
            if (_hit.collider.TryGetComponent<BasePlayerController>(out BasePlayerController _player))
            {
                if (!_player.GetComponent<BaseEnemyAI>())
                {
                    _player.TakeDamage(_damage);
                    if (_player._currentHealth <= 0 || !_target.activeSelf)
                    {
                        _target = null;
                    }
                }
                else
                {
                    _player.GetComponent<BaseEnemyAI>().TakeDamage(_damage, gameObject);
                    if (_player._currentHealth <= 0 || !_target.activeSelf)
                    {
                        _target = null;
                    }
                }
            }
            else
            {
                if (_hit.collider.TryGetComponent<BaseEnemyAI>(out BaseEnemyAI _enemy))
                {
                    _enemy.TakeDamage(_damage, gameObject);
                    if (_enemy._currentHealth <= 0 || !_target.activeSelf)
                    {
                        _target = null;
                    }
                }
            }
        }
    }

    void Dead()
    {
        if (_targetType == TargetType.player)
        {
            FindFirstObjectByType<GameUIManager>().WaveSpawn();
        }
        gameObject.SetActive(false);
    }


    public void MassSlowDown(float _slowDown)
    {
        _slowDown = Mathf.Clamp(_slowDown, 0f, 100f);
        _agent.speed = _speed * (1f - _slowDown / 100f);
        _animator.speed = 1f - (_slowDown / 100f);
    }


    public void PermanentSlow(float _slowdown)
    {
        _agent.speed -= (_speed * _slowdown / 100);
        _animator.speed -= (1f * _slowdown / 100);
    }

    public void PermanentSpeedUp(float _speedUp)
    {
        _agent.speed += (_speed * _speedUp / 100);
        _animator.speed += (1f * _speedUp / 100);
    }

    public void SlowDown(float _duration, float _slowDown = 0, float _delay = 0, bool _continous = false, float _activationDelay = 0)
    {
        StartCoroutine(SlowDownTempo(_duration, _slowDown, _delay, _continous, _activationDelay));
    }

    IEnumerator SlowDownTempo(float _duration, float _slowDown, float _delay, bool _continous, float _activationDelay)
    {
        yield return new WaitForSeconds(_activationDelay);
        _agent.speed -= (_agent.speed * _slowDown / 100);
        _animator.speed -= (_animator.speed * _slowDown / 100);
        if (_continous)
        {
            yield return new WaitUntil(_gameUIManager._player.HaveForce);
        }
        else
        {
            yield return new WaitForSeconds(_duration);
        }
        if (_delay > 0)
        {
            while (_animator.speed != 1f || _agent.speed != _speed)
            {
                _agent.speed = Mathf.MoveTowards(_agent.speed, _speed, _delay * Time.deltaTime);
                _animator.speed = Mathf.MoveTowards(_animator.speed, 1f, _delay * Time.deltaTime);
            }
            _agent.speed = _speed;
            _animator.speed = 1f;
            yield return null;
        }
        else
        {
            _agent.speed = _speed;
            _animator.speed = 1f;
        }
    }

    public void Freeze(float _freezeTime, float _StunAfterFrezze = 0)
    {
        StartCoroutine(FreezeTempo(_freezeTime, _StunAfterFrezze));
    }

    IEnumerator FreezeTempo(float _freezeTime, float _StunAfterFrezze)
    {
        _agent.speed = 0f;
        _agent.isStopped = true;
        _animator.speed = 0f;
        yield return new WaitForSeconds(_freezeTime);
        if(_StunAfterFrezze > 0)
        {
            _notMove = true;
            _agent.speed = _speed;
            _animator.speed = 1f;
            _agent.isStopped = false;
            yield return new WaitForSeconds(_StunAfterFrezze);
            _notMove = false;
        }
        else
        {
            _agent.speed = _speed;
            _animator.speed = 1f;
            _agent.isStopped = false;
        }
    }
    
    public void Normal()
    {
        if (_targetType == TargetType.player)
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _notMove = false;
            _agent.speed = _speed;
            _animator.speed = 1f;
        }
    }
    public IEnumerator DestroySequence(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        if (FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget == gameObject.transform)
        {
            FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget = _gameUIManager._player.transform;
        }
        Destroy(gameObject);
    }
}
