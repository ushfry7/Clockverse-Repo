using UnityEngine;

public class MoodTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                CryMoodSelected();
                break;
            case 2:
                RawMoodSelected();
                break;
            case 3:
                AntMoodSelected();
                break;
            case 4:
                ButMoodSelected();
                break;
            default:
                BasicMoodSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Cry_Mood();
                break;
            case 2:
                Raw_Mood();
                break;
            case 3:
                Ant_Mood();
                break;
            case 4:
                But_Mood();
                break;
            default:
                Basic_Mood();
                break;
        }
    }

    void BasicMoodSelected()
    {
        // Base Mood
        // Recording Duration: 5s
        // Recharge: 5s
        // Operation:
        // Record the player's actions for 5s.
        // Spawn a clone that replays the recorded actions.
        // Cooldown does not begin during replay.
        TempoValues(5f, 15f, 1, 0, false);
    }

    void Basic_Mood()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void CryMoodSelected()
    {
        // Cry
        // Recording Duration: Continuous
        // Recharge: None
        // Operation:
        // Constantly replay the player's actions from 4s ago.
        TempoValues(35f, 5f, 1, 0, false);
    }

    void Cry_Mood()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration, 3);
        }
    }

    public void RawMoodSelected()
    {
        // Raw
        // Recording Duration: 1s
        // Recharge: 5s
        // Operation:
        // Instantly replay the last 1s of recorded actions.
        // Clone attacks can damage the player.
        TempoValues(75f, 15f, 3, 0, true);
    }

    int _vexCloneAmount = 1;
    void Raw_Mood()
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

    public void AntMoodSelected()
    {
        // Ant
        // Recording Duration: 1s
        // Clone Windup: 1s
        // Recharge: 2s
        // Operation:
        // Reduce recording duration to 1s.
        // Clone has a 1s windup before replaying.
        TempoValues(6f, 1f, 1, 0, false);
    }

    void Ant_Mood()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void ButMoodSelected()
    {
        // But
        // Recording Duration: 5s
        // Recharge: 5s
        // Operation:
        // Reactivate Mood to pause the replay.
        // Activate again to continue the replay.
        // Duration continues ticking while replay is paused.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void But_Mood()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }
}
