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
    IEnumerator GoldWork()
    {
        while (true)
        {
            if (!_player.HaveForce())
            {
                EpochCall();
            }
            yield return new WaitUntil(_player.HaveForce);
        }
    }

    void BasicGoldSelected()
    {
        StartCoroutine(GoldWork());
        //While the player is standing still, time slows down incredibly.
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
        StartCoroutine(GoldWork());
        //        When you start moving:

        //Enemy time takes 1 second to return to normal.
        //Enemies are only slowed by 90 %.
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
        StartCoroutine(GoldWork());
        //        Time becomes fully stopped
        //Gold takes 0.2s to activate
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
        //        While holding the activation:

        //You can move for up to 3s
        //Time remains slowed during that movement
        //Has a 9s cooldown
        //Cannot stop time while cooldown is active
        TempoValues(9f, 3f, 1, 0, false);
    }

    void But_Gold()
    {
        if (_charges > 0)
        {
            _charges--;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.SlowDown(_tempoEffectDuration, 90f, 3f, true, 0f);
            }
            StartCoroutine(CoolDownPeriod());
        }
    }

    public void ZenGoldSelected()
    {
        //Gold becomes passive:
        //Enemy time is permanently slowed by 25 %
        //All other Gold mechanics are disabled.

    }

    void Zen_Gold()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            enemy.PermanentSlow(25f);
        }
    }
}
