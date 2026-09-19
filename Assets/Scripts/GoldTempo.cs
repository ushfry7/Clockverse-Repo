using System.Collections;
using UnityEngine;

public class GoldTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                RueGoldSelected();
                break;
            case 2:
                RawGoldSelected();
                break;
            case 3:
                ButGoldSelected();
                break;
            case 4:
                ZenGoldSelected();
                break;
            default:
                BasicGoldSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                //Rue_Gold();
                break;
            case 2:
                //Raw_gold();
                break;
            case 3:
                But_Gold();
                break;
            case 4:
                //Zen_Gold();
                break;
            default:
                //Basic_Gold();
                break;
        }
    }
    IEnumerator GoldWork()
    {
        while (true)
        {
            if (!_player.HaveForce())
            {
                switch (_epochSelected)
                {
                    case 1:
                        Rue_Gold();
                        break;
                    case 2:
                        Raw_gold();
                        break;
                    case 3:
                        But_Gold();
                        break;
                    case 4:
                        Zen_Gold();
                        break;
                    default:
                        Basic_Gold();
                        break;
                }
            }
            yield return new WaitUntil(_player.HaveForce);
        }
    }

    void BasicGoldSelected()
    {
        // ================= GOLD =================

        // Base Gold
        // Duration: While standing still
        // Recharge: None
        // Operation:
        // While player is stationary, enemy time slows down dramatically.
        // Moving disables the effect.
        StartCoroutine(GoldWork());      
    }

    void Basic_Gold()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.SlowDown(_tempoEffectDuration, 90f, 0, true);
        }
    }

    public void RueGoldSelected()
    {
        // ================= RUE =================

        // Rue
        // Duration: While standing still
        // Recharge: None
        // Operation:
        // Enemy time returns to normal 1s after the player starts moving.
        // Maximum enemy slowdown = 90%.
        StartCoroutine(GoldWork());
    }

    void Rue_Gold()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.SlowDown(_tempoEffectDuration, 90f, 1, true);
        }
    }

    public void RawGoldSelected()
    {
        // ================= RAW =================

        // Raw
        // Duration: While standing still
        // Recharge: None
        // Activation Delay: 0.2s
        // Operation:
        // Completely stop enemy time.
        // Takes 0.2s to activate.
        StartCoroutine(GoldWork());
    }

    void Raw_gold()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.SlowDown(_tempoEffectDuration, 100f, 0, true, 0.2f);
        }
    }

    public void ButGoldSelected()
    {
        // ================= BUT =================

        // But
        // Duration: Up to 3s movement
        // Recharge: 9s
        // Operation:
        // Hold activation to gain up to 3s of movement.
        // Enemy time remains slowed while moving.
        // Cannot stop time while cooldown is active.
        TempoValues(9f, 3f, 1, 0, false);
    }

    void But_Gold()
    {
        if (_charges > 0)
        {
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 90f, 3f, true, 0f);
            }
            StartCoroutine(CoolDownPeriod());
        }
    }

    public void ZenGoldSelected()
    {
        // ================= ZEN =================

        // Zen
        // Duration: Passive
        // Recharge: None
        // Operation:
        // Permanently slow enemy time by 25%.
        // Disables all other Gold behavior.
        Zen_Gold();
    }

    void Zen_Gold()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            enemy.PermanentSlow(25f);
        }
    }
}
