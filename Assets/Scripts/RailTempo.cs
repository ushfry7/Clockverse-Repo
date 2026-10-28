using UnityEngine;

public class RailTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                GapRailSelected();
                break;
            case 2:
                AceRailSelected();
                break;
            case 3:
                SinRailSelected();
                break;
            case 4:
                PayRailSelected();
                break;
            default:
                BasicRailSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Gap_Rail();
                break;
            case 2:
                Ace_Rail();
                break;
            case 3:
                Sin_Rail();
                break;
            case 4:
                Pay_Rail();
                break;
            default:
                Basic_Rail();
                break;
        }
    }

    void BasicRailSelected()
    {
        // Base Rail
        // Duration: Instant reset
        // Recharge: 5 minutes
        // Operation:
        // Reset player state to the beginning of the current stage.
        // Leave a clone behind at the activation point.
        // Allow upgrades to be shuffled once per stage.
        // Cooldown is reduced by 50% upon entering a new stage.
        TempoValues(300f, 15f, 1, 0, false);
    }

    void Basic_Rail()
    {
        if (_charges > 0)
        {
            if (_gameUIManager.StageChange())
            {
                _tempoCooldown /= 2f;
            }
            StartCoroutine(CoolDownPeriod());
            _player.RailTempo();
        }
    }

    public void GapRailSelected()
    {
        // Gap
        // Duration: Instant reset
        // Recharge: 60s
        // Operation:
        // Reset player state to the beginning of the current stage.
        // Do not leave a clone behind.
        // Reduce cooldown to 60s.
        TempoValues(60f, 5f, 1, 0, false);
    }

    void Gap_Rail()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RailTempo(false);
        }
    }

    public void AceRailSelected()
    {
        // Ace
        // Duration: Instant reset
        // Recharge: 5 minutes
        // Operation:
        // If player health is below 25% when activated,
        // the clone is healed to full health.
        // Otherwise, health is not reset.
        TempoValues(300f, 15f, 1, 0, true);
    }

    void Ace_Rail()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RailTempo();
        }
    }

    public void SinRailSelected()
    {
        // Sin
        // Duration: Instant reset
        // Recharge: 30s
        // Operation:
        // Reduce cooldown to 30s.
        // Clone does not activate for 30s.
        TempoValues(30f, 1f, 1, 0, false);
    }

    void Sin_Rail()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.RailTempo(true, 30f);
        }
    }

    public void PayRailSelected()
    {
        // Pay
        // Duration: Charge-based
        // Recharge: Damage-based
        // Operation:
        // Taking 1 damage grants 1% charge.
        // Dealing 1 damage grants 0.2% charge.
        // Damage dealt and taken by clones also counts.
        TempoValues(20f, 5f, 1, 0, false);
        _currentRecharge = 100;
        _gameUIManager.CooldownTextTypeChange(false);
    }

    void Pay_Rail()
    {
        if (_currentRecharge >= 100)
        {
            _player.RailTempo();
            _currentRecharge = 0;
        }
    }
}

