using System.Collections;
using UnityEngine;

public class TempoSystem : MonoBehaviour
{
    public BasePlayerController _player;
    public GameUIManager _gameUIManager;
    public int _tempoSelected, _epochSelected = 0, _chargeLimit = 1;
    public float _currentCoolDownTimer, _tempoCooldown = 20f, _tempoEffectDuration = 5f, _effectPenality, _effectPenalityResetDuration, _timeToActivate = 0;
    public bool _autoCall = false, _canCancelEarly = false, _timeInfluence = false, _canCalledEarly;
    public int _charges, _currentRecharge;

    void Start()
    {
        _player = FindFirstObjectByType<BasePlayerController>();
        _gameUIManager = FindFirstObjectByType<GameUIManager>();
    }

    public virtual void EpochSelect()
    {
        _currentCoolDownTimer = 0f;
        _charges = _chargeLimit;
    }

    public virtual void EpochCall()
    {

    }

    public void TempoValues(float _tempoCoolD, float _effectDuration, int _charge, int _activationTime, bool _autoC, bool _callEarly = false)
    {
        _tempoCooldown = _tempoCoolD;
        _tempoEffectDuration = _effectDuration;
        _chargeLimit = _charge;
        _charges = _chargeLimit;
        _timeToActivate = _activationTime;
        _autoCall = _autoC;
        _canCalledEarly = _callEarly;
    }

    public float _decreaseAmount = 1;
    public virtual IEnumerator CoolDownPeriod()
    {
        _charges -= 1;
        _currentCoolDownTimer = _tempoCooldown;
        while (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer -= _decreaseAmount;
            _gameUIManager.TimerUpdate();
            yield return new WaitForSeconds(1f);
            if (_currentCoolDownTimer <= 0)
            {
                if (_charges < _chargeLimit)
                {
                    _charges += 1;
                }
            }
        }
        _gameUIManager.TimerUpdate();
    }

    public IEnumerator LargeCoolDownPeriod()
    {
        _charges -= 1;
        _currentCoolDownTimer += _tempoCooldown;
        while (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer--;
            _gameUIManager.TimerUpdate();
            yield return new WaitForSeconds(1f);
            if (_currentCoolDownTimer < _tempoCooldown * 2 && _charges == 0)
            {
                _charges++;
            }
            else if (_currentCoolDownTimer < _tempoCooldown && _charges == 1)
            {
                _charges++;
            }
            else if (_currentCoolDownTimer <= 0 && _charges == 2)
            {
                _charges++;
            }
        }
        _gameUIManager.TimerUpdate();
    }

    public virtual void StopALL()
    {
        StopAllCoroutines();
    }

    private int _currentCloneIndex = -1;
    public GameObject CloneSwap(BasePlayerController[] _clones)
    {
        if (_clones == null || _clones.Length == 0)
            return null;
        for (int i = 0; i < _clones.Length; i++)
        {
            _currentCloneIndex = (_currentCloneIndex + 1) % _clones.Length;
            GameObject _clone = _clones[_currentCloneIndex].gameObject;
            if (_clone != null && _clone.activeInHierarchy)
                return _clone;
        }
        return null;
    }
}
