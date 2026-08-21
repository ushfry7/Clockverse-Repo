using System.Collections;
using UnityEngine;

public class TempoSystem : MonoBehaviour
{
    public BasePlayerController _player;
    public GameUIManager _gameUIManager;
    public int _tempoSelected, _epochSelected = 0, _chargeLimit = 1;
    public float _currentCoolDownTimer, _tempoCooldown = 20f, _tempoEffectDuration = 5f, _effectPenality, _effectPenalityResetDuration, _timeToActivate = 0;
    public bool _autoCall = false, _canCancelEarly = false, _timeInfluence = false;
    public int _charges, _currentRecharge;

    void Start()
    {
        _player = FindFirstObjectByType<BasePlayerController>();
        _gameUIManager = FindFirstObjectByType<GameUIManager>();
    }

    public virtual void EpochSelect()
    {

    }

    public virtual void EpochCall()
    {

    }

    public void TempoValues(float _tempoCoolD, float _effectDuration, int _charge, int _activationTime, bool _autoC)
    {
        _tempoCooldown = _tempoCoolD;
        _tempoEffectDuration = _effectDuration;
        _chargeLimit = _charge;
        _charges = _chargeLimit;
        _timeToActivate = _activationTime;
        _autoCall = _autoC;
    }


    public virtual IEnumerator CoolDownPeriod()
    {
        _charges -= 1;
        _currentCoolDownTimer = _tempoCooldown;
        while (_currentCoolDownTimer > 0)
        {
            _currentCoolDownTimer--;
            _gameUIManager.TimerUpdate();
            yield return new WaitForSeconds(1f);
            if (_currentCoolDownTimer == 0)
            {
                if (_charges < _chargeLimit)
                {
                    _charges += 1;
                }
            }
        }
    }
}
