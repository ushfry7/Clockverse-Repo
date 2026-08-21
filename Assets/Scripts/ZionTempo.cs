using UnityEngine;

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
        //Default: Gradually double the speed of time for yourself for 20s, decaying after it ends. Recharges over 40s.
        TempoValues(40f, 20f, 1, 0, false);
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
        //Ant: Duration, windup, wind-down and cooldown reduced to a quarter of original length.
        TempoValues(10f, 5f, 1, 0, false);
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
        //Cry: Your speed of time is permanently increased by 25%. Disables all other aspects of the tempo.
        TempoValues(10f, 5f, 1, 0, true);
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
        //Gem: Your speed of time is instead tripled, but otherwise reduced by 20% when inactive.
        TempoValues(40f, 20f, 1, 0, false);
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
        //But: Tempo can be deactivated at any time, keeping unused charge, but cooldown is increased to 60s.
        TempoValues(60f, 20f, 1, 0, false);
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
