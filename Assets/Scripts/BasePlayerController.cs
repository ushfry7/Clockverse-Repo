using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BasePlayerController : MonoBehaviour
{
    [SerializeField] GameObject _cloneModal, _cloneAiModal;
    public Transform _orientation, _followCamera;
    [SerializeField] LayerMask _groundLayerMask;
    float _verticalValue, _horizontalValue, _rotationValue = 12f, _moveForce, _groundCheckDistance = 0.2f, _maxHealth = 100f;
    [SerializeField] float _drag = 7f, _walkForce = 47f, _sprintForce = 95, _downwardForce = 30f, _hitMaxDistance = 3f, _baseDamage = 10f, _currentDamage;
    public float _currentHealth, _speedMultiplier = 1f;
    public int _comboCount = 2;
    public Animator _animator;
    Rigidbody _rb;
    public Vector3 _inputDir;
    bool _isGrounded;
    public bool _canMove = true, _clone = false, _ai;
    GameUIManager _gameUIManager;
    BasePlayerController _playerClone;
    Vector3 _startPos;
    BaseEnemyAI _playerAiClone;
    public GameObject _swapClone;
    public bool _attacking = false, _imune = false, _imuneHit = false;


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
        _currentDamage = _baseDamage;
        _startPos = transform.position;
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
        if (_canMove)
        {
            _verticalValue = Input.GetAxis("Vertical");
            _horizontalValue = Input.GetAxis("Horizontal");
            if (Time.timeScale == 1f)
            {
                // Walk / Run 
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _moveForce = _sprintForce;
                }
                else
                {
                    _moveForce = _walkForce;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Attack();
                }
                if (Input.GetKey(KeyCode.E))
                {
                    _animator.SetBool("Block", true);
                }
                else
                {
                    _animator.SetBool("Block", false);
                }
                if (Input.GetKeyDown(KeyCode.Escape) && !_clone)
                {
                    _gameUIManager.SettingMenu();
                }

                if (Input.GetKeyDown(KeyCode.Space) && !_clone)
                {
                    _gameUIManager._tempoSystem.EpochCall();
                    if (FindFirstObjectByType<DirtTempo>().enabled && _gameUIManager._tempoSystem._charges == 0)
                    {
                        DirtCloneSwap();
                    }
                    if (FindFirstObjectByType<DiscTempo>().enabled && _gameUIManager._tempoSystem._charges == 0)
                    {
                        DiscCloneSwap();
                    }
                    _gameUIManager.TimerUpdate();
                }
            }
        }
    }


    void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayerMask);
        if (!_isGrounded)
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
        HealthUpdate();
    }

    public bool HaveForce()
    {
        return _inputDir.magnitude > 0;
    }

    void HealthUpdate()
    {
        HealthRegan();
        _gameUIManager.HealthUpdate(_currentHealth / _maxHealth);
        if (_currentHealth <= 0 && !_clone)
        {
            _gameUIManager.SettingMenu();
        }
    }

    void HealthRegan()
    {
        if (_currentHealth < _maxHealth && !_attacking && _inputDir.magnitude != 0)
        {
            _currentHealth += 8f * Time.deltaTime;
        }
    }

    public bool _tookHit = false;
    public void TakeDamage(float _damageValue)
    {
        if (FindFirstObjectByType<StarTempo>().enabled)
        {
            FindFirstObjectByType<StarTempo>().ChargeIncrease(25);
        }
        if (!_imune)
        {
            if (!_animator.GetBool("Block"))
            {
                _tookHit = true;
                if (_currentHealth > _damageValue)
                {
                    _currentHealth -= _damageValue;
                }
                else
                {
                    _currentHealth = 0f;
                }
                HealthUpdate();
                FindFirstObjectByType<TempoSystem>()._currentRecharge += 25;
                _gameUIManager.RechargeUpdate();
            }
        }
        else
        {
            _imuneHit = true;
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
                    enemy.TakeDamage(_currentDamage, gameObject);
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

    //public void HaltTempo(float value)
    //{
    //    StartCoroutine(HaltSequence(value));
    //}

    //IEnumerator HaltSequence(float value)
    //{
    //    _canMove = false;
    //    _animator.speed = 0f;
    //    yield return new WaitForSeconds(value);
    //    _canMove = true;
    //    _animator.speed = 1f;
    //}

    public void PlayerSlowDown(float _slowDown)
    {
        _speedMultiplier -= _slowDown / 100;
        _animator.speed -= _slowDown / 100;
    }
    public void RuleTempo(float _duration)
    {
        StartCoroutine(RuleSequence(_duration));
    }

    IEnumerator RuleSequence(float _duration)
    {
        _imune = true;
        _imuneHit = false;
        yield return new WaitForSeconds(_duration);
        _imune = false;
    }

    public void ZionTempo(float _duration, float _maxSpeed, float _returnSpeed)
    {
        StartCoroutine(ZionSequence(_duration, _maxSpeed, _returnSpeed));
    }

    IEnumerator ZionSequence(float _duration, float _maxSpeed, float _returnSpeed)
    {
        _speedMultiplier = _maxSpeed;
        _animator.speed = _maxSpeed;
        yield return new WaitForSeconds(_duration);
        _speedMultiplier = _returnSpeed;
        _animator.speed = _returnSpeed;
    }

    public void RushTempo(float _duration, float _maxSpeed, float _returnSpeed)
    {
        StartCoroutine(RushSequence(_duration, _maxSpeed, _returnSpeed));
    }

    IEnumerator RushSequence(float _duration, float _maxSpeed, float _returnSpeed)
    {
        _speedMultiplier = _maxSpeed;
        _animator.speed = _maxSpeed;
        yield return new WaitForSeconds(_duration);
        _speedMultiplier = _returnSpeed;
        _animator.speed = _returnSpeed;
    }

    public void Normal()
    {
        _canMove = true;
        _speedMultiplier = 1f;
        _animator.speed = 1f;
    }

    public void DirtCloneCheck()
    {
        if (_clone)
        {
            _followCamera.gameObject.SetActive(false);
            _followCamera = FindFirstObjectByType<CinemachineOrbitalFollow>().gameObject.transform;
        }
    }

    void DirtCloneSwap()
    {
        if (_swapClone == null)
        {
            _swapClone = gameObject;
        }
        else
        {
            _swapClone = FindFirstObjectByType<DirtTempo>().CloneSwap(FindObjectsByType<BasePlayerController>(FindObjectsSortMode.None));
            if (_swapClone != null)
            {
                _followCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = _swapClone.transform;
            }
        }
    }

    public void DirtTempo(float _duration, int _cloneNumber = 1)
    {
        for (int i = 0; i < _cloneNumber; i++)
        {
            _playerClone = Instantiate(_cloneModal, transform.position + transform.forward * 2f, transform.rotation).GetComponent<BasePlayerController>();
            _playerClone.StartCoroutine(_playerClone.DirtSequence(_duration));
        }
    }

    public IEnumerator DirtSequence(float _duration)
    {
        _clone = true;
        DirtCloneCheck();
        yield return new WaitForSeconds(_duration);
        if (FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget == gameObject.transform)
        {
            FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget = _gameUIManager._player.transform;
        }
        Destroy(gameObject);
    }

    void DiscCloneSwap()
    {
        if (_swapClone == null)
        {
            _swapClone = _gameUIManager._player.gameObject;
        }
        else if (_swapClone != _gameUIManager._player.gameObject)
        {
            ResetDiscClone(_swapClone);
        }
        _swapClone = FindFirstObjectByType<DiscTempo>().CloneSwap(FindObjectsByType<BasePlayerController>(FindObjectsSortMode.None));
        if (_swapClone == null)
            return;

        bool _isPlayer = _swapClone == _gameUIManager._player.gameObject;
        _swapClone.GetComponent<BasePlayerController>().enabled = true;
        if (!_isPlayer)
        {
            _swapClone.GetComponent<BaseEnemyAI>().enabled = false;
            _swapClone.GetComponent<BasePlayerController>().enabled = true;
            Animator _animator = _swapClone.GetComponent<Animator>();
            _animator.SetBool("Run", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Block", false);
            _animator.SetInteger("Force", 0);
        }
        _followCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget = _swapClone.transform;
    }

    void ResetDiscClone(GameObject _clone)
    {
        _clone.GetComponent<BaseEnemyAI>().enabled = true;
        _clone.GetComponent<BasePlayerController>().enabled = false;
        Animator _animator = _clone.GetComponent<Animator>();
        _animator.SetBool("Run", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Block", false);
        _animator.SetInteger("Force", 0);
    }

    public void DiscTempo(float _duration, bool _unLimited = false, int _cloneNumber = 1, float _speedDifference = 0, float _speedUp = 0)
    {
        for (int i = 0; i < _cloneNumber; i++)
        {
            _playerAiClone = Instantiate(_cloneAiModal, transform.position + transform.forward * 2f, transform.rotation).GetComponent<BaseEnemyAI>();
            _playerAiClone.TargetTypeEnemy();
            _playerAiClone.PermanentSlow(_speedDifference);
            _playerAiClone.PermanentSpeedUp(_speedUp);
            _playerAiClone.GetComponent<BasePlayerController>()._speedMultiplier = 1f - (_speedDifference / 100f);
            _playerAiClone.GetComponent<BasePlayerController>()._clone = true;
            _playerAiClone.GetComponent<BasePlayerController>()._followCamera = _followCamera;
            if (!_unLimited)
            {
                _playerAiClone.StartCoroutine(_playerAiClone.DestroySequence(_duration));
            }
        }
    }

    public void RailTempo(bool _clone = true, float _freezeTime = 0f)
    {
        if (_clone)
        {
            _playerAiClone = Instantiate(_cloneAiModal, transform.position + transform.forward * 2f, transform.rotation).GetComponent<BaseEnemyAI>();
            _playerAiClone.TargetTypeEnemy();
            if (_freezeTime != 0)
            {
                _playerAiClone.Freeze(_freezeTime);
            }
        }
        transform.position = _startPos;
    }

}