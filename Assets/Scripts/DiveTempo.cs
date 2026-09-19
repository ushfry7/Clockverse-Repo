using System.Collections;
using UnityEngine;

public class DiveTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                HexDiveSelected();
                break;
            case 2:
                IceDiveSelected();
                break;
            case 3:
                RawDiveSelected();
                break;
            case 4:
                YenDiveSelected();
                break;
            default:
                BasicDiveSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Hex_Dive();
                break;
            case 2:
                Ice_Dive();
                break;
            case 3:
                Raw_Dive();
                break;
            case 4:
                Yen_Dive();
                break;
            default:
                Basic_Dive();
                break;
        }
    }

    public void BasicDiveSelected()
    {
        // ================= DIVE =================

        // Base Dive
        // Windup: 2s
        // Duration: 10s
        // Recharge: 40s
        // Operation:
        // After a 2s windup, completely stop time.
        // Can deactivate early and retain the remaining charge.
        TempoValues(40f, 10f, 1, 2, false);
    }

    void Basic_Dive()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f, 0, false, 2f);
            }
        }
    }

    public void HexDiveSelected()
    {
        // ================= HEX =================

        // Hex
        // Windup: 2s
        // Duration: 10s
        // Recharge: 40s
        // Recovery: 5s
        // Operation:
        // After deactivation, time gradually returns to normal over 5s.
        // Player time speed during recovery = 90%
        // (because active slowdown is 100%).
        TempoValues(40f, 10f, 1, 0, false);
    }

    void Hex_Dive()
    {
        if (_charges > 0)
        {
            StartCoroutine(Hex_Dive_Sequence());
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f, 5f, false, 2f);
            }
        }
    }

    IEnumerator Hex_Dive_Sequence()
    {
        yield return new WaitForSeconds(12f);
        _player.PlayerSlowDown(90f);
        yield return new WaitForSeconds(5f);
        _player.Normal();
    }

    public void IceDiveSelected()
    {
        // ================= ICE =================

        // Ice
        // Windup: 2s
        // Duration: 10s
        // Recharge: 40s
        // Operation:
        // Each enemy killed during the time stop,
        // if they had more than 25% max HP,
        // reduces cooldown by 10s.
        // If no qualifying enemy is killed,
        // cooldown becomes 60s after deactivation.
        _timeInfluence = true;
        TempoValues(40f, 10f, 1, 2, false);
    }

    ArrayList _healthyEnemy = new ArrayList();
    void Ice_Dive()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f, 0f, false, 2f);
                if (enemy.IsHealthy())
                {
                    _healthyEnemy.Add(enemy);
                }
            }
            StartCoroutine(Ice_Sequence());
        }
    }

    IEnumerator Ice_Sequence()
    {
        bool _continue = true;
        while (_charges == 0 && _continue)
        {
            foreach (BaseEnemyAI enemy in _healthyEnemy)
            {
                if (!enemy.gameObject.activeSelf)
                {
                    _tempoCooldown = 30f;
                    _continue = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        _tempoCooldown = 60f;
    }

    public void RawDiveSelected()
    {
        // ================= RAW =================

        // Raw
        // Windup: None
        // Duration: 12s total
        // Recharge: 40s
        // Operation:
        // Early deactivation is disabled.
        // The normal 2s windup becomes part of the active duration.
        // Time stop begins immediately.
        TempoValues(40f, 12f, 1, 0, true);
    }

    void Raw_Dive()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f, 0f, false, 0f);
            }
        }
    }

    public void YenDiveSelected()
    {
        // ================= YEN =================

        // Yen
        // Full Charge: 60s
        // Full Duration: 15s
        // Operation:
        // Can activate while charging.
        // Effect duration depends on current charge.
        // 0% charge = weakest effect.
        // 100% charge = 15s duration.
        // Charge continues to build while on cooldown.
        //At full charge, Dive lasts 15 seconds total.
        TempoValues(60f, 15f, 1, 0, false, true);
    }

    void Yen_Dive()
    {
        _tempoEffectDuration = 15f * (_tempoCooldown - _currentCoolDownTimer / 60);
        if (_canCalledEarly && _currentCoolDownTimer > 0)
        {

            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
            _currentCoolDownTimer = _tempoCooldown;
        }
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 100f);
            }
        }
    }

}
