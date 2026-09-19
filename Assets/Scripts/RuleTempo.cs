using System.Collections;

public class RuleTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                IreRuleSelected();
                break;
            case 2:
                AntRuleSelected();
                break;
            case 3:
                VexRuleSelected();
                break;
            case 4:
                IceRuleSelected();
                break;
            default:
                BasicRuleSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ire_Rule();
                break;
            case 2:
                Ant_Rule();
                break;
            case 3:
                Vex_Rule();
                break;
            case 4:
                Ice_Rule();
                break;
            default:
                Basic_Rule();
                break;
        }
    }

    void BasicRuleSelected()
    {
        // ================= RULE =================

        // Base Rule
        // Duration: 0.5s
        // Recharge: 2s
        // Operation:
        // Instantly skip 0.5s of time.
        // You cannot be affected by anything during the skipped time.
        TempoValues(2f, 0.5f, 1, 0, false);
    }

    void Basic_Rule()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public void IreRuleSelected()
    {
        // ================= IRE =================

        // Ire
        // Duration: 1s
        // Recharge: 5s
        // Operation:
        // Skip 1s of time instead.
        // The time skip only affects yourself.
        TempoValues(5f, 1f, 1, 0, false);
    }

    void Ire_Rule()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public void AntRuleSelected()
    {
        // ================= ANT =================

        // Ant
        // Duration: 0.25s
        // Recharge: 1s
        // Operation:
        // Skip 0.25s of time.
        // Cooldown is reduced by half.
        TempoValues(1f, 0.25f, 1, 0, false);
    }

    void Ant_Rule()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public void VexRuleSelected()
    {
        // ================= VEX =================

        // Vex
        // Duration: 0.5s
        // Charges: 3
        // Recharge: 8s per charge
        // Operation:
        // Adds 2 extra charges.
        // Each charge recharges independently after 8s.
        TempoValues(8f, 0.5f, 3, 0, false);
    }

    void Vex_Rule()
    {
        if (_charges == _chargeLimit)
        {
            StartCoroutine(LargeCoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
        else if (_charges > 0)
        {
            _currentCoolDownTimer += _tempoCooldown;
            _charges--;
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public void IceRuleSelected()
    {
        // ================= ICE =================

        // Ice
        // Duration: Continuous while held
        // Recharge: 0.5s normally / 5s if damaged
        // Operation:
        // Can be held to continuously activate Rule.
        // If you take damage within 1s of using Rule,
        // cooldown becomes 5s.
        // Otherwise, cooldown becomes 0.5s.
        TempoValues(2f, 0.5f, 1, 0, false);
    }

    void Ice_Rule()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public override IEnumerator CoolDownPeriod()
    {
        if (_currentCoolDownTimer > 0)
        {
            if (_player._imuneHit)
            {
                _tempoCooldown = 5f;
            }
            else
            {
                _tempoCooldown = 0.5f;
            }
        }
        return base.CoolDownPeriod();
    }
}