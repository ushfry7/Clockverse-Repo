using UnityEngine;

public class SwapTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                IceSwapSelected();
                break;
            case 2:
                RawSwapSelected();
                break;
            case 3:
                SinSwapSelected();
                break;
            case 4:
                AceSwapSelected();
                break;
            default:
                BasicSwapSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ice_Swap();
                break;
            case 2:
                Raw_Swap();
                break;
            case 3:
                Sin_Swap();
                break;
            case 4:
                Ace_Swap();
                break;
            default:
                Basic_Swap();
                break;
        }
    }

    void BasicSwapSelected()
    {
        // Base Swap
        // Duration: Not specified
        // Recharge: 5s
        // Operation:
        // Switch between two versions of yourself from different timelines.
        // Preserve each version's individual state.
        TempoValues(5f, 15f, 1, 0, false);
    }

    void Basic_Swap()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void IceSwapSelected()
    {
        // Ice
        // Duration: Not specified
        // Recharge: 5s
        // Operation:
        // Inactive clone's cooldowns recover 50% faster.
        // Attacking while the clone is inactive adds 5% of the
        // wasted cooldown to its attack cooldown.
        // Can recharge even when fully charged.
        TempoValues(35f, 5f, 1, 0, false);
    }

    void Ice_Swap()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration, 3);
        }
    }

    public void RawSwapSelected()
    {
        // Raw
        // Duration: Not specified
        // Recharge: 5s
        // Operation:
        // Clone can damage enemies and the player.
        // If no other targets are available, clone can target itself.
        TempoValues(75f, 15f, 3, 0, true);
    }

    int _vexCloneAmount = 1;
    void Raw_Swap()
    {
        if (_charges > 1)
        {
            _charges--;
            _player.DirtTempo(_tempoEffectDuration, _vexCloneAmount);
            _vexCloneAmount++;

        }
        else if (_charges == 1)
        {
            _vexCloneAmount = 4;
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration, _vexCloneAmount);
            _vexCloneAmount = 1;
        }
    }

    public void SinSwapSelected()
    {
        // Sin
        // Duration: Not specified
        // Recharge: None
        // Operation:
        // Remove Swap's cooldown.
        // The previous version is halted for 1s after swapping.
        TempoValues(6f, 1f, 1, 0, false);
    }

    void Sin_Swap()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void AceSwapSelected()
    {
        // Ace
        // Duration: Not specified
        // Recharge: 5s normally; 60s if clone dies
        // Operation:
        // If clone has less health than the player:
        // Clone cooldowns recover 2x faster.
        // Clone health regenerates 4x faster.
        // Otherwise:
        // Player cooldowns recover 50% slower.
        // Player health regeneration is disabled.
        // If clone dies, wait 60s before the clone regenerates.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void Ace_Swap()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }
}
