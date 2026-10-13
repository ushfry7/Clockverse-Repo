using UnityEngine;

public class DiscTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                CryDiscSelected();
                break;
            case 2:
                BegDiscSelected();
                break;
            case 3:
                VexDiscSelected();
                break;
            case 4:
                FoeDiscSelected();
                break;
            default:
                BasicDiscSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Cry_Disc();
                break;
            case 2:
                Beg_Disc();
                break;
            case 3:
                Vex_Disc();
                break;
            case 4:
                Foe_Disc();
                break;
            default:
                Basic_Disc();
                break;
        }
    }

    void BasicDiscSelected()
    {
        // ================= DISC =================

        // Base Disc
        // Duration: 10s
        // Recharge: 15s
        // Operation:
        // Create a clone of the player from another timeline.
        // Clone acts independently from the player.
        // Clone has its own movement and attacks.
        // Player can swap controls with the clone.
        TempoValues(25f, 10f, 1, 0, false);
    }

    void Basic_Disc()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DiscTempo(_tempoEffectDuration);
        }
    }

    public void CryDiscSelected()
    {
        // Cry
        // Duration: Permanent
        // Recharge: None
        // Operation:
        // Clone remains permanently active.
        // Clone acts independently.
        // Clone moves 50% slower.
        TempoValues(10f, 5f, 1, 0, false);
        if (_charges > 0)
        {
            _charges = 0;
            _gameUIManager.TimerUpdate();
            _player.DiscTempo(_tempoEffectDuration, true, 1, 50f);
        }
    }

    void Cry_Disc()
    {

    }

    public void BegDiscSelected()
    {
        // Beg
        // Duration: Active while used
        // Recharge: 1s
        // Operation:
        // Clone acts 4x faster.
        // Player is stopped while the clone is active.
        // Cannot swap controls.
        // Cooldown reduced to 1s.
        TempoValues(10f, 5f, 1, 0, true);
    }

    void Beg_Disc()
    {
        if (_charges > 0)
        {
            _player._canMove = false;
            StartCoroutine(CoolDownPeriod());
            _player.DiscTempo(_tempoEffectDuration, true, 1, 0, 400f);
        }
    }

    public void VexDiscSelected()
    {
        // Vex
        // Duration: 10s
        // Charges: 5
        // Recharge: 30s
        // Operation:
        // Adds 4 extra charges.
        // Each activation creates one additional clone.
        // All clones exist simultaneously.
        TempoValues(40f, 10f, 5, 0, false);
    }

    void Vex_Disc()
    {
        if (_charges > 1)
        {
            _charges--;
            _player.DiscTempo(_tempoEffectDuration);
        }
        else if (_charges == 1)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DiscTempo(_tempoEffectDuration);
        }
    }

    public void FoeDiscSelected()
    {
        // Foe
        // Duration: 5s
        // Recharge: 15s
        // Operation:
        // Clone constantly targets the enemy closest to the cursor.
        // Clone automatically aggros and attacks that target.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void Foe_Disc()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DiscTempo(_tempoEffectDuration);
        }
    }

    private int _currentCloneIndex = -1;
    public GameObject DiscClone(BasePlayerController[] _clones)
    {
        if (_clones == null || _clones.Length == 0)
            return null;
        for (int i = 0; i < _clones.Length; i++)
        {
            _currentCloneIndex = (_currentCloneIndex + 1) % _clones.Length;
            GameObject _clone = _clones[_currentCloneIndex].gameObject;
            if (_clone != null && _clone.activeInHierarchy)
                return _clone;
        }
        return null;
    }
}
