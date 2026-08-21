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
        //Effect: Slow enemy time.
        //Enemy time speed → 50 %
        //Duration: 6s
        //Maximum 3 charges
        //Each charge recharges after 30s
        //Charges recharge simultaneously
        TempoValues(30f, 6f, 3, 0, false);
    }

    void Basic_Path()
    {
        if (_charges > 1)
        {
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
        else if (_charges == 1 && _currentCoolDownTimer <= 0)
        {
            _charges--;
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 0.5f);
            }
        }
    }

    public void VexPathSelected()
    {
        //        Vex — Irritation
        //        Charge amount doubled
        //Cooldown doubled
        TempoValues(60f, 6f, 6, 0, false);
    }

    void Vex_Path()
    {
        if (_charges > 1)
        {
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
        else if (_charges == 1 && _currentCoolDownTimer <= 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 50f);
            }
        }
    }

    public void CryPathSelected()
    {
        //Every 6 seconds, enemy time switches between:
        //        Normal
        //   ↕
        //33 % slower
        TempoValues(6f, 6f, 1, 0, true);
        Cry_Path();
    }

    void Cry_Path()
    {
        if (_charges > 0)
        {
            StartCoroutine(Cry_Path_Sequence());
        }
    }

    IEnumerator Cry_Path_Sequence()
    {
        while (true)
        {
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 33f);
            }
            yield return new WaitForSeconds(_tempoEffectDuration);
        }
    }


    public void AntPathSelected()
    {
        //        Only 1 charge
        //Recharge → 8s
        TempoValues(80f, 6f, 1, 0, false);
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
        //        Recharge → 20s
        //        Charges cannot stack
        //Recharge timers are not simultaneous
        TempoValues(20f, 6f, 1, 0, false);
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
