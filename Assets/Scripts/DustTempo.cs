using UnityEngine;

public class DustTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                ButDustSelected();
                break;
            case 2:
                AntDustSelected();
                break;
            case 3:
                RawDustSelected();
                break;
            case 4:
                FoeDustSelected();
                break;
            default:
                BasicDustSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                But_Dust();
                break;
            case 2:
                Ant_Dust();
                break;
            case 3:
                Raw_Dust();
                break;
            case 4:
                Foe_Dust();
                break;
            default:
                Basic_Dust();
                break;
        }
    }

    void BasicDustSelected()
    {
        // Base Dust
        // Rewind Duration: 5s
        // Recharge: 20s
        // Operation:
        // Rewind the player's own state by 5s.
        // The rest of the world does not automatically rewind.
        TempoValues(5f, 15f, 1, 0, false);
    }

    void Basic_Dust()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void ButDustSelected()
    {
        // But
        // Rewind Duration: Up to 5s
        // Recharge: 20s
        // Operation:
        // Rewind can be stopped at any time.
        // Cooldown remains 20s regardless of rewind duration.
        TempoValues(35f, 5f, 1, 0, false);
    }

    void But_Dust()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration, 3);
        }
    }

    public void AntDustSelected()
    {
        // Ant
        // Rewind Duration: 1s
        // Recharge: 4s
        // Operation:
        // Divide rewind duration by 5.
        // Divide cooldown by 5.
        TempoValues(75f, 15f, 3, 0, true);
    }

    int _vexCloneAmount = 1;
    void Ant_Dust()
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

    public void RawDustSelected()
    {
        // Raw
        // Rewind Duration: Until manually stopped
        // Recharge: 20s
        // Operation:
        // Rewind continues until activated again.
        // Rewind at normal speed.
        // Enemies can hit the player during rewind.
        TempoValues(6f, 1f, 1, 0, false);
    }

    void Raw_Dust()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void FoeDustSelected()
    {
        // Foe
        // Rewind Duration: 5s
        // Recharge: 20s
        // Operation:
        // Rewind the enemy closest to the cursor instead of the player.
        // The enemy retains its current health.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void Foe_Dust()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }
}