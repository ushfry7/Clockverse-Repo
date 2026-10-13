using System.Collections;
using UnityEngine;

public class HalfTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                IreHaltSelected();
                break;
            case 2:
                JoyHaltSelected();
                break;
            case 3:
                CryHaltSelected();
                break;
            case 4:
                VexHaltSelected();
                break;
            default:
                BasicHaltSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ire_Halt();
                break;
            case 2:
                Joy_Halt();
                break;
            case 3:
                Cry_Halt();
                break;
            case 4:
                Vex_Halt();
                break;
            default:
                Basic_Halt();
                break;
        }
    }

    public void BasicHaltSelected()
    {
        // ================= HALT =================

        // Base Halt
        // Duration: 5s
        // Recharge: 20s
        // Operation:
        // Stop enemy time for 5s.
        // Player time is unaffected.
        TempoValues(25f, 5f, 1, 0, false);
    }

    void Basic_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }

    public void IreHaltSelected()
    {

        // ================= IRE =================

        // Ire
        // Duration: 7.5s
        // Recharge: 28s
        // Operation:
        // Increase enemy time-stop duration to 7.5s.
        // Recharge time increased to 28s.
        TempoValues(35.5f, 7.5f, 1, 0, false);
    }

    void Ire_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }

    public void JoyHaltSelected()
    {
        // ================= JOY =================

        // Joy
        // Duration: 3s
        // Recharge: Ability-based
        // Operation:
        // Duration reduced to 3s.
        // Using another ability charges Halt by 50% of that ability's cooldown.
        // Example: 10s ability cooldown = +5s Halt charge.
        _timeInfluence = true;
        TempoValues(23f, 3f, 1, 0, false);
    }

    void Joy_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }

    public void CryHaltSelected()
    {
        // ================= CRY =================

        // Cry
        // Duration: 5s
        // Recharge: 15s
        // Operation:
        // Cooldown reduced to 15s.
        // Automatically activates immediately when the cooldown finishes.
        TempoValues(20f, 5f, 1, 0, true);
        StartCoroutine(Cry_halt_Sequence());
    }

    void Cry_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }

    IEnumerator Cry_halt_Sequence()
    {
        yield return new WaitForSeconds(1f);
        while (Time.timeScale > 0)
        {
            Cry_Halt();
            yield return new WaitForSeconds(1f);
        }
    }

    public void VexHaltSelected()
    {
        // ================= VEX =================

        // Vex
        // Duration: 5s
        // Charges: 3
        // Recharge: 40s per charge
        // Operation:
        // Adds 2 extra charges.
        // Each charge has its own 40s recharge.
        TempoValues(45f, 5f, 3, 0, false);
    }

    void Vex_Halt()
    {
        if (_charges == _chargeLimit)
        {
            StartCoroutine(LargeCoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
        else if (_charges > 0)
        {
            _currentCoolDownTimer += _tempoCooldown;
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }
}
