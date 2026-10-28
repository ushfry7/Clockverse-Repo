using UnityEngine;

public class DirtTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                IreDirtSelected();
                break;
            case 2:
                VexDirtSelected();
                break;
            case 3:
                AntDirtSelected();
                break;
            case 4:
                RawDirtSelected();
                break;
            default:
                BasicDirtSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ire_Dirt();
                break;
            case 2:
                Vex_Dirt();
                break;
            case 3:
                Ant_Dirt();
                break;
            case 4:
                Raw_Dirt();
                break;
            default:
                Basic_Dirt();
                break;
        }
    }

    void BasicDirtSelected()
    {
        // ================= DIRT =================

        // Base Dirt
        // Duration: 15s
        // Recharge: 30s
        // Operation:
        // Create a clone of the player from another timeline.
        // Clone mirrors the player's actions.
        // Clone copies player movement and attacks.
        // Player and clone can swap controls.
        TempoValues(45f, 15f, 1, 0, false);
    }

    void Basic_Dirt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void IreDirtSelected()
    {
        // Ire
        // Duration: 5s
        // Recharge: 30s
        // Operation:
        // Create 2 additional clones.
        // Total number of clones = 3.
        TempoValues(35f, 5f, 1, 0, false);
    }

    void Ire_Dirt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration, 3);
        }
    }

    public void VexDirtSelected()
    {
        // Vex
        // Duration: 15s
        // Charges: 3
        // Recharge: 60s
        // Operation:
        // Adds 2 extra charges.
        // Each activation doubles the player and all active clones.
        // Maximum = 8 copies.
        // Activations cannot happen simultaneously.
        TempoValues(75f, 15f, 3, 0, true);
    }

    int _vexCloneAmount = 1;
    void Vex_Dirt()
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

    public void AntDirtSelected()
    {
        // Ant
        // Duration: 1s
        // Recharge: 5s
        // Operation:
        // Reduce clone duration to 1s.
        // Reduce cooldown to 5s.
        TempoValues(6f, 1f, 1, 0, false);
    }

    void Ant_Dirt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }

    public void RawDirtSelected()
    {
        // Raw
        // Duration: 15s
        // Recharge: 5s
        // Operation:
        // Cooldown reduced to 5s.
        // Dirt can recharge while the clone is active.
        // Clone can damage the player.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void Raw_Dirt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.DirtTempo(_tempoEffectDuration);
        }
    }
}