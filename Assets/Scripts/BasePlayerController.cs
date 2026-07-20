using System.Collections;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{
    [SerializeField] Transform _orientation, _followCamera;
    [SerializeField] LayerMask _groundLayerMask;
    float _verticalValue, _horizontalValue, _rotationValue = 12f, _maxHealth = 100f,
    _currentStamina, _maxStamina = 100f, _moveForce, _groundCheckDistance = 0.2f;
    [SerializeField]
    float _drag = 7f, _walkForce = 47f, _sprintForce = 95, _jumpForce = 10f, _downwardForce = 30f,
        _staminaRegain = 20f, _staminaDrain = 8f, _speedMultiplier = 1f, _hitMaxDistance = 3f, _baseDamage = 10f, _currentDamage,
        _tempoCooldown = 30f, _tempoEffectDuration = 5f;
    public float _currentCoolDownTimer, _currentHealth;
    public int _comboCount = 2, _abilitySelected;
    Animator _animator;
    Rigidbody _rb;
    Vector3 _inputDir;
    bool _isGrounded;
    bool _canMove = true;
    GameUIManager _gameUIManager;
    public bool _attacking = false;

    void Start()
    {
        ComponentAssignment();
    }

    void ComponentAssignment()
    {
        _animator = GetComponent<Animator>();
        _gameUIManager = FindFirstObjectByType<GameUIManager>();
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.linearDamping = _drag;
        _moveForce = _walkForce;
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;
        _currentDamage = _baseDamage;
    }
    // Update is called once per frame
    void Update()
    {
        KeyInputs();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public virtual void KeyInputs()
    {
        _verticalValue = Input.GetAxis("Vertical");
        _horizontalValue = Input.GetAxis("Horizontal");
        if (Time.timeScale == 1f)
        {
            // Walk / Run 
            if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
            {
                _moveForce = _sprintForce;
                _currentStamina -= _staminaDrain * Time.deltaTime;
            }
            else
            {
                _moveForce = _walkForce;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKey(KeyCode.E))
            {
                _animator.SetBool("Block", true);
            }
            else
            {
                _animator.SetBool("Block", false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameUIManager.SettingMenu();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_currentCoolDownTimer == 0)
                {
                    if (_abilitySelected == 1)
                    {
                        SpeedTime();
                    }
                    else if (_abilitySelected == 2)
                    {
                        SlowDownTime();
                    }
                    else if (_abilitySelected == 3)
                    {
                        FreezeTime();
                    }
                    StartCoroutine(CoolDownPeriod());
                }
            }
        }
    }


    void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayerMask);
        if (!_isGrounded && !_animator.GetBool("Jump"))
        {
            _rb.AddForce(Vector3.down * _downwardForce, ForceMode.Force);
        }
    }

    void Movement()
    {
        if (_canMove)
        {
            _orientation.forward = transform.position - new Vector3(_followCamera.position.x, transform.position.y, _followCamera.position.z);
            _inputDir = _orientation.forward * _verticalValue + _orientation.right * _horizontalValue;
            if (_inputDir.magnitude != 0)
            {
                _animator.SetInteger("Force", _moveForce == _sprintForce ? 2 : 1);
                if (_animator.GetBool("Jump"))
                {
                    _moveForce = _walkForce;
                }
                _rb.AddForce(_inputDir.normalized * (_moveForce * _speedMultiplier), ForceMode.Force);
                transform.forward = Vector3.Lerp(transform.forward, _inputDir.normalized, _rotationValue * Time.deltaTime);
            }
            else
            {
                _moveForce = _walkForce;
                _animator.SetInteger("Force", 0);
            }
        }
        else
        {
            _animator.SetInteger("Force", 0);
        }
        StaminaUpdate();
        HealthUpdate();
    }

    void StaminaUpdate()
    {
        if (_moveForce != _sprintForce)
        {
            if (_currentStamina != _maxStamina)
            {
                _currentStamina += _staminaRegain * Time.deltaTime;
                if (_currentStamina > _maxStamina)
                {
                    _currentStamina = _maxStamina;
                }
            }
        }
        _gameUIManager.StaminaUpdate(_currentStamina / _maxStamina);
    }

    void HealthUpdate()
    {
        HealthRegan();
        _gameUIManager.HealthUpdate(_currentHealth / _maxHealth);
        if(_currentHealth <= 0)
        {
            _gameUIManager.SettingMenu();
        }
    }

    void HealthRegan()
    {
        if (_currentHealth < _maxHealth && !_attacking && _inputDir.magnitude != 0)
        {
            _currentHealth += _staminaRegain * Time.deltaTime;
        }
    }

    void Jump()
    {
        if (_isGrounded && _canMove)
        {
            _animator.SetBool("Jump", true);
        }
    }

    void JumpForce()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    void JumpReset()
    {
        _animator.SetBool("Jump", false);
    }

    public void TakeDamage(float _damageValue)
    {
        if (!_animator.GetBool("Block"))
        {
            _currentHealth -= _damageValue;
            HealthUpdate();
        }
    }

    void Attack()
    {
        if (_isGrounded)
        {
            _canMove = false;
            if (!_animator.GetBool("Attack"))
            {
                _attacking = true;
                _animator.SetBool("Attack", true);
                _comboCount++;
                if (_comboCount == 3)
                {
                    _comboCount = 0;
                }
                _currentDamage = _baseDamage + (_baseDamage * _comboCount);
                _animator.SetInteger("Strike", _comboCount);
            }
        }
    }

    void Strike()
    {
        Collider[] hits = Physics.OverlapSphere(_orientation.position, _hitMaxDistance);

        foreach (Collider hit in hits)
        {
            Vector3 dir = (hit.transform.position - _orientation.position).normalized;

            if (Vector3.Angle(transform.forward, dir) <= 45f)
            {
                if (hit.TryGetComponent(out BaseEnemyAI enemy))
                {
                    enemy.TakeDamage(_currentDamage);
                }
            }
        }
    }

    void AttackReset()
    {
        _canMove = true;
        _attacking = false;
        _animator.SetBool("Attack", false);
    }

    IEnumerator CoolDownPeriod()
    {
        _currentCoolDownTimer = _tempoCooldown;
        while (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer--;
            _gameUIManager.TimerUpdate();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpeedTime()
    {
        StartCoroutine(Speed(_tempoEffectDuration));
    }

    IEnumerator Speed(float value)
    {
        _speedMultiplier += _speedMultiplier / 2f;
        _animator.speed += _animator.speed / 2f;
        yield return new WaitForSeconds(value);
        _speedMultiplier = 1f;
        _animator.speed = 1f;
    }

    void SlowDownTime()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.SlowDown(_tempoEffectDuration);
        }
    }

    void FreezeTime()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.Freeze(_tempoEffectDuration);
        }
    }
}
