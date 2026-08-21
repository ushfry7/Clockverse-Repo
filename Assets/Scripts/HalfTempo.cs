using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HalfTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                IreHaltSelected();
                break;
            case 2:
                JoyHaltSelected();
                break;
            case 3:
                CryHaltSelected();
                break;
            case 4:
                VexHaltSelected();
                break;
            default:
                BasicHaltSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Ire_Halt();
                break;
            case 2:
                Joy_Halt();
                break;
            case 3:
                Cry_Halt();
                break;
            case 4:
                Vex_Halt();
                break;
            default:
                Basic_Halt();
                break;
        }
    }

    public void BasicHaltSelected()
    {
        //Base — Halt
        //Purpose: Stop your own time.
        //Duration: 5 seconds
        //Cooldown: 20 seconds
        //It affects you, not the enemies.
        TempoValues(20f, 5f, 1, 0, false);
    }

    void Basic_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.HaltTempo(_tempoEffectDuration);
        }
    }

    public void IreHaltSelected()
    {
        //Ire = Rage
        //Halt duration: 5s → 7.5s
        //Cooldown: 20s → 28s
        TempoValues(28f, 7.5f, 1, 0, false);
    }

    void Ire_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.HaltTempo(_tempoEffectDuration);
        }
    }

    public void JoyHaltSelected()
    {
        //The duration becomes shorter:
        //Halt duration = 3s
        //But instead of relying only on normal recharge, using another ability charges Halt by 50 % of that ability's cooldown
        _timeInfluence = true;
        TempoValues(20f, 3f, 1, 0, false);
    }

    void Joy_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            _player.HaltTempo(_tempoEffectDuration);
        }
    }

    public void CryHaltSelected()
    {
        //Cry = Sorrow
        //Cry makes the Tempo passive.
        //Cooldown becomes 15s
        //Halt automatically activates when it comes off cooldown.
        TempoValues(15f, 5f, 1, 0, true);
        Cry_Halt();
    }

    void Cry_Halt()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
        }
    }

    public void VexHaltSelected()
    {
        //Vex = Irritation
        //Adds 2 extra charges
        //Cooldown becomes 40s per charge.
        TempoValues(40f, 5f, 2, 0, false);
    }

    void Vex_Halt()
    {
        if (_charges > 0)
        {
            _player.HaltTempo(_tempoEffectDuration);
        }
        else
        {
            StartCoroutine(CoolDownPeriod());
        }
    }

    public override IEnumerator CoolDownPeriod()
    {
        if (_epochSelected == 3)
        {
            Cry_Halt();
        }
        return base.CoolDownPeriod();
    }
}
