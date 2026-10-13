using System.Collections;
using UnityEngine;

public class RushTempo : TempoSystem
{
    float _multiplier = 4f, _normal = 1f;
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                GemRushSelected();
                break;
            case 2:
                AntRushSelected();
                break;
            case 3:
                RueRushSelected();
                break;
            case 4:
                YenRushSelected();
                break;
            default:
                BasicRushSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Gem_Rush();
                break;
            case 2:
                Ant_Rush();
                break;
            case 3:
                Rue_Rush();
                break;
            case 4:
                Yen_Rush();
                break;
            default:
                Basic_Rush();
                break;
        }
    }

    void BasicRushSelected()
    {
        // ================= RUSH =================

        // Base Rush
        // Duration: Up to 1s while held
        // Recharge: 10s
        // Operation:
        // Increase player's time speed to 4x.
        // Effect continues while the button is held, up to 1s.
        TempoValues(11f, 1f, 1, 0, false);
    }

    void Basic_Rush()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RushTempo(_tempoEffectDuration, _multiplier, _normal);
        }
    }
    public void GemRushSelected()
    {
        // ================= GEM =================

        // Gem
        // Duration: Up to 1s
        // Recharge: 7.5s
        // Operation:
        // Player time speed = 6x.
        // Outside Rush, player time speed = 75%.
        TempoValues(8.5f, 1f, 1, 0, false);
        _multiplier = 6f;
        _normal = 0.75f;
        _player._speedMultiplier = _normal;
        _player._animator.speed = _normal;
    }

    void Gem_Rush()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RushTempo(_tempoEffectDuration, _multiplier, _normal);
        }
    }

    public void AntRushSelected()
    {
        // ================= ANT =================

        // Ant
        // Duration: Up to 1s
        // Recharge: 4s
        // Operation:
        // Player time speed = 2x.
        TempoValues(5f, 1f, 1, 0, false);
    }

    void Ant_Rush()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RushTempo(_tempoEffectDuration, 2f, 1f);
        }
    }

    public void RueRushSelected()
    {
        // ================= RUE =================

        // Rue
        // Duration: Up to 1s
        // Recharge: 5s
        // Operation:
        // Each activation reduces the time-speed multiplier by 1x.
        // Multiplier returns to normal after 10s without using Rush.
        TempoValues(6f, 1f, 3, 0, true);
    }

    float _rueMaxMulti = 4f, _rueCooldownTime = 10f, _currentRueMulti = 4f, _currentRueCooldowntimer = 0f;
    Coroutine _rueSequence;
    void Rue_Rush()
    {
        _currentRueCooldowntimer = _rueCooldownTime;
        if (_rueSequence == null || _currentRueCooldowntimer <= 0)
        {
            _rueSequence = StartCoroutine(RueResetSequence());
        }
        if (_charges == _chargeLimit)
        {
            StartCoroutine(LargeCoolDownPeriod());
            _player.RushTempo(_tempoEffectDuration, _currentRueMulti, 1f);
            _currentRueMulti--;
        }
        else if (_charges > 0)
        {
            _currentCoolDownTimer += _tempoCooldown;
            _charges--;
            _player.RushTempo(_tempoEffectDuration, _currentRueMulti, 1f);
            _currentRueMulti--;
        }
    }

    IEnumerator RueResetSequence()
    {
        while (_currentRueCooldowntimer > 0)
        {
            _currentRueCooldowntimer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        _currentRueMulti = _rueMaxMulti;
    }

    public void YenRushSelected()
    {
        // ================= YEN =================

        // Yen
        // Duration: Depends on current charge
        // Recharge: 12s
        // Operation:
        // At 0% charge = 25% effectiveness.
        // Every 1% charge increases effectiveness by 2%.
        // Maximum effectiveness = 225%.
        TempoValues(13f, 1f, 1, 0, false);
    }

    float _effectiveness = 0, _effectivenessMultiplier = 0.25f;
    void Yen_Rush()
    {
        if (_charges > 0)
        {
            _effectiveness = _effectivenessMultiplier + (100f * 0.02f);
            StartCoroutine(CoolDownPeriod());
            _player.RushTempo(_tempoEffectDuration, _multiplier * _effectiveness, _normal);
        }
    }

    public override void StopALL()
    {
        base.StopALL();
        _currentRueMulti = _rueMaxMulti;
    }
}
