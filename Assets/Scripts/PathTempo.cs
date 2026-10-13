using System.Collections;
using UnityEngine;

public class PathTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                VexPathSelected();
                break;
            case 2:
                CryPathSelected();
                break;
            case 3:
                AntPathSelected();
                break;
            case 4:
                GapPathSelected();
                break;
            default:
                BasicPathSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Vex_Path();
                break;
            case 2:
                Cry_Path();
                break;
            case 3:
                Ant_Path();
                break;
            case 4:
                Gap_Path();
                break;
            default:
                Basic_Path();
                break;
        }
    }

    public void BasicPathSelected()
    {
        // ================= PATH =================

        // Base Path
        // Duration: 6s
        // Charges: 3
        // Recharge: 30s per charge
        // Operation:
        // Reduce enemy time speed to 50%.
        // Charges recharge independently/simultaneously.
        TempoValues(36f, 6f, 3, 0, false);
    }

    void Basic_Path()
    {
        if (_charges == _chargeLimit)
        {
            StartCoroutine(LargeCoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
        else if (_charges > 0)
        {
            _currentCoolDownTimer += _tempoCooldown;
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
    }

    public void VexPathSelected()
    {
        // ================= VEX =================

        // Vex
        // Duration: 6s
        // Charges: 6
        // Recharge: 60s per charge
        // Operation:
        // Double the charge count and recharge time.
        TempoValues(66f, 6f, 6, 0, false);
    }

    void Vex_Path()
    {
        if (_charges == _chargeLimit)
        {
            StartCoroutine(LargeCoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
        else if (_charges > 0)
        {
            _currentCoolDownTimer += _tempoCooldown;
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
    }

    public void CryPathSelected()
    {
        // ================= CRY =================

        // Cry
        // Duration: Passive
        // Recharge: None
        // Operation:
        // Every 6s, enemy time speed switches between:
        // Normal (100%) and 33% slower (67%).
        TempoValues(12f, 6f, 1, 0, true);
        StartCoroutine(Cry_Path_Sequence());
    }

    void Cry_Path()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 33f);
            }
        }
    }

    IEnumerator Cry_Path_Sequence()
    {
        yield return new WaitForSeconds(1f);
        while (Time.timeScale > 0)
        {
            Cry_Path();
            yield return new WaitForSeconds(1f);
        }
    }


    public void AntPathSelected()
    {
        // ================= ANT =================

        // Ant
        // Duration: 6s
        // Charges: 1
        // Recharge: 8s
        // Operation:
        // Only one charge.
        // Reduce recharge time to 8s.
        TempoValues(14f, 6f, 1, 0, false);
    }

    void Ant_Path()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
    }

    public void GapPathSelected()
    {
        // ================= GAP =================

        // Gap
        // Duration: 6s
        // Charges: 1
        // Recharge: 20s
        // Operation:
        // Cooldown reduced to 20s.
        // Charges cannot stack.
        // Next charge cannot recharge until current cooldown finishes.
        TempoValues(26f, 6f, 1, 0, false);
    }

    void Gap_Path()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
    }
}
