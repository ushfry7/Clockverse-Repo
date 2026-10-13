public class ZionTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                AntZionSelected();
                break;
            case 2:
                CryZionSelected();
                break;
            case 3:
                GemZionSelected();
                break;
            case 4:
                ButZionSelected();
                break;
            default:
                BasicZionSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ant_Zion();
                break;
            case 2:
                Cry_Zion();
                break;
            case 3:
                Gem_Zion();
                break;
            case 4:
                But_Zion();
                break;
            default:
                Basic_Zion();
                break;
        }
    }

    void BasicZionSelected()
    {
        // ================= ZION =================

        // Base Zion
        // Duration: 20s
        // Recharge: 40s
        // Operation:
        // Gradually increase player's time speed up to 2x.
        // After the effect ends, time speed decays back to normal.
        TempoValues(60f, 20f, 1, 0, false);
    }

    void Basic_Zion()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.ZionTempo(_tempoEffectDuration, 2f, 1f);
        }
    }

    public void AntZionSelected()
    {
        // ================= ANT =================

        // Ant
        // Duration: 5s
        // Recharge: 10s
        // Operation:
        // Duration, windup, wind-down and cooldown are reduced to 25%.
        // Time speed effect remains the same.
        TempoValues(15f, 5f, 1, 0, false);
    }

    void Ant_Zion()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.ZionTempo(_tempoEffectDuration, 2f, 1f);
        }
    }

    public void CryZionSelected()
    {
        // ================= CRY =================

        // Cry
        // Duration: Passive
        // Recharge: None
        // Operation:
        // Permanently increase player's time speed by 25%.
        // Disables all other Zion behavior.
        TempoValues(0f, 0f, 0, 0, true);
        Cry_Zion();
    }

    void Cry_Zion()
    {
        if (_charges > 0)
        {
            _charges--;
            _player._speedMultiplier = 1.25f;
            _player._animator.speed = 1.25f;
        }
    }

    public void GemZionSelected()
    {
        // ================= GEM =================

        // Gem
        // Duration: 20s
        // Recharge: 40s
        // Operation:
        // Player time speed increases to 3x instead of 2x.
        // Outside Zion, player's time speed is reduced by 20%.
        TempoValues(60f, 20f, 1, 0, false);
        _player._speedMultiplier = 0.8f;
        _player._animator.speed = 0.8f;
    }

    void Gem_Zion()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.ZionTempo(_tempoEffectDuration, 3f, 0.8f);
        }
    }

    public void ButZionSelected()
    {
        // ================= BUT =================

        // But
        // Duration: Up to 20s
        // Recharge: 60s
        // Operation:
        // Can deactivate Zion early.
        // Unused charge is retained.
        // Recharge time increased to 60s.
        TempoValues(80f, 20f, 1, 0, false);
    }

    void But_Zion()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.ZionTempo(_tempoEffectDuration, 2f, 1f);
        }
    }
}
