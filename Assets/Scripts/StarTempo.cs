using System.Collections;
using UnityEngine;

public class StarTempo : TempoSystem
{

    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                RueStarSelected();
                break;
            case 2:
                FoeStarSelected();
                break;
            case 3:
                AntStarSelected();
                break;
            case 4:
                PayStarSelected();
                break;
            default:
                BasicStarSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Rue_Star();
                break;
            case 2:
                Foe_Star();
                break;
            case 3:
                Ant_Star();
                break;
            case 4:
                Pay_Star();
                break;
            default:
                Basic_Star();
                break;
        }
    }

    public void BasicStarSelected()
    {
        // ================= STAR =================

        // Base Star
        // Duration: 2s
        // Recharge: 8s
        // Operation:
        // Stop enemy time for 2s.
        // Player time is unaffected.
        TempoValues(10f, 2f, 1, 0, false);
    }

    void Basic_Star()
    {
        StartCoroutine(CoolDownPeriod());
        FreezeEnemyTime();
    }

    public void RueStarSelected()
    {
        // ================= RUE =================

        // Rue
        // Duration: Starts at 2s
        // Recharge: 3s
        // Operation:
        // Cooldown reduced to 3s.
        // Every activation reduces duration by 0.5s.
        // Duration penalty resets after 12s without activation.
        TempoValues(5f, 2f, 1, 0, false);
    }

    float _rueTempoMaxDuration = 2f, _rueCooldownTime = 12f, _currentRueCooldowntimer = 0f;
    Coroutine _rueSequence;

    void Rue_Star()
    {
        if (_charges > 0)
        {
            _currentRueCooldowntimer = _rueCooldownTime;
            if (_rueSequence == null || _currentRueCooldowntimer <= 0)
            {
                _rueSequence = StartCoroutine(RueResetSequence());
            }
            if (_tempoEffectDuration == 2f)
            {
                _tempoCooldown = 3f;
                StartCoroutine(CoolDownPeriod());
                FreezeEnemyTime();
                _tempoEffectDuration -= 0.5f;
            }
            else if (_tempoEffectDuration > 0.5f)
            {
                StartCoroutine(CoolDownPeriod());
                FreezeEnemyTime();
                _tempoEffectDuration -= 0.5f;
            }
            else if (_tempoEffectDuration == 0.5f)
            {
                StartCoroutine(CoolDownPeriod());
                FreezeEnemyTime();
            }
        }
    }

    IEnumerator RueResetSequence()
    {
        while (_currentRueCooldowntimer > 0)
        {
            _currentRueCooldowntimer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        _tempoEffectDuration = _rueTempoMaxDuration;
    }

    public void FoeStarSelected()
    {
        // ================= FOE =================

        // Foe
        // Duration: 3s
        // Recharge: 8s
        // Operation:
        // Only affect the enemy closest to the cursor/aim.
        // Increase duration to 3s.
        TempoValues(11f, 3f, 1, 0, false);
    }

    void Foe_Star()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            Foe_Star_target();
        }
    }

    void Foe_Star_target()
    {
        Vector3 screenCenter = new Vector3(
           Screen.width * 0.5f,
           Screen.height * 0.5f,
           0f
       );

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        Collider[] enemies = Physics.OverlapSphere(
            ray.origin + ray.direction * (30f * 0.5f),
            30f * 0.5f
        );

        BaseEnemyAI closestEnemy = null;
        float closestScreenDistance = float.MaxValue;

        foreach (Collider col in enemies)
        {
            BaseEnemyAI enemy = col.GetComponentInParent<BaseEnemyAI>();

            if (enemy == null)
                continue;

            Vector3 viewportPosition =
                Camera.main.WorldToViewportPoint(enemy.transform.position);
            if (viewportPosition.z <= 0f)
                continue;
            float screenDistance = Vector2.Distance(
                new Vector2(viewportPosition.x, viewportPosition.y),
                new Vector2(0.5f, 0.5f)
            );

            if (screenDistance < closestScreenDistance)
            {
                closestScreenDistance = screenDistance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.Freeze(_tempoEffectDuration);
        }
    }

    public void AntStarSelected()
    {
        // ================= ANT =================

        // Ant
        // Duration: 1s
        // Recharge: 4s
        // Operation:
        // Duration halved from 2s to 1s.
        // Cooldown halved from 8s to 4s.
        TempoValues(5f, 1f, 1, 0, false);
    }

    void Ant_Star()
    {
        StartCoroutine(CoolDownPeriod());
        FreezeEnemyTime();
    }

    public void PayStarSelected()
    {
        // ================= PAY =================

        // Pay
        // Duration: 2s
        // Recharge: Damage-based
        // Operation:
        // Taking damage gives 25% charge.
        // Dealing 1 damage gives 2% charge.
        // Does not recharge passively.
        // Cannot recharge while Star is active.
        TempoValues(0, 2f, 1, 0, false);
        ChargeIncrease(100);
    }

    public void ChargeIncrease(int _amount)
    {
        _currentRecharge += _amount;
        if (_currentRecharge >= 100f)
        {
            _charges = _chargeLimit;
        }
        else
        {
            _charges = 0;
        }
        _gameUIManager.TimerUpdate();
    }

    void Pay_Star()
    {
        if (_currentRecharge >= 100)
        {
            FreezeEnemyTime();
            _currentRecharge = 0;
            _gameUIManager.TimerUpdate();
        }
    }

    void FreezeEnemyTime()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.Freeze(_tempoEffectDuration);
        }
    }
}
