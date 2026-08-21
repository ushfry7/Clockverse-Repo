using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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
        //Instantly skip a half second of time.You cannot be affected by anything in the skipped time.Recharges after 2s.
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
        //Ire: Skip 1 second of time instead, and only for yourself.Cooldown increased to 5s.
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
        //Ant: Skip a quarter of a second of time instead, but reduce cooldown by half.
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
        //Vex: 2 extra charges, but cooldown increased to 8s per charge.
        TempoValues(8f, 0.5f, 3, 0, false);
    }

    void Vex_Rule()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RuleTempo(_tempoEffectDuration);
        }
    }

    public void IceRuleSelected()
    {
        //Gold becomes passive:
        //Ice: When taking damage within 1 second of using the Tempo, increase cooldown to 5s. 
        //Otherwise, reduce cooldown to half a second. Can now be held to continuously activate.
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
        if (_currentCoolDownTimer > 1)
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