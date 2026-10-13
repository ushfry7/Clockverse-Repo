using System.Collections;
using UnityEngine;

public class MassTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                DenMassSelected();
                break;
            case 2:
                RawMassSelected();
                break;
            case 3:
                CryMassSelected();
                break;
            case 4:
                IceMassSelected();
                break;
            default:
                BasicMassSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Den_Mass();
                break;
            case 2:
                Raw_Mass();
                break;
            case 3:
                Cry_Mass();
                break;
            case 4:
                Ice_Mass();
                break;
            default:
                Basic_Mass();
                break;
        }
    }

    public void BasicMassSelected()
    {
        // ================= MASS =================

        // Base Mass
        // Duration: 10s
        // Recharge: 30s
        // Operation:
        // Time speed is proportional to distance from the player.
        // Closer enemies = more affected by time slowdown.
        // Farther enemies = less affected.
        // Exact radius / slowdown curve is not specified in GDD.
        TempoValues(40f, 10f, 1, 0, false);
    }

    void Basic_Mass()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            StartCoroutine(MassCoroutine(90f));
        }
    }

    public void DenMassSelected()
    {
        // ================= DEN =================

        // Den
        // Duration: 10s
        // Recharge: 30s
        // Operation:
        // Flip the distance relationship.
        // Time is slowed depending on how far the target is from things.
        // TODO: Define exact distance calculation.
        TempoValues(40f, 10f, 1, 0, false);
    }

    void Den_Mass()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            StartCoroutine(MassCoroutine(100f, true));
        }
    }

    public void RawMassSelected()
    {
        // ================= RAW =================

        // Raw
        // Duration: 10s
        // Recharge: 30s
        // Operation:
        // Slow time for everything around the player.
        // Player also receives 20% of the slowdown.
        TempoValues(40f, 10f, 1, 0, false);
    }

    void Raw_Mass()
    {
        if (_charges > 0)
        {
            _player.RushTempo(10f, 0.8f, 1f);
            StartCoroutine(CoolDownPeriod());
            StartCoroutine(MassCoroutine(90f));
        }
    }

    public void CryMassSelected()
    {
        // ================= CRY =================

        // Cry
        // Passive: YES
        // Duration: Permanent
        // Recharge: None
        // Operation:
        // Mass is always active.
        // Maximum slowdown is reduced by 50%.
        TempoValues(4f, 1f, 0, 0, false);
        StartCoroutine(MassCoroutine(50f, false, true));
    }

    void Cry_Mass()
    {

    }

    public void IceMassSelected()
    {
        // ================= ICE =================

        // Ice
        // Duration: 10s
        // Recharge: 30s
        // Operation:
        // Maximum slowdown becomes a complete time stop.
        // Enemies sufficiently far away are sped up instead.
        // Effect radius is decreased.
        TempoValues(40f, 10f, 1, 0, false);
    }

    void Ice_Mass()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            StartCoroutine(MassCoroutine(100f));
        }
    }


    private IEnumerator MassCoroutine(float _maxSlowDown, bool _opposite = false, bool _permanent = false)
    {
        float _duration = _tempoEffectDuration;
        float _timer = 0f;
        while (_permanent || _timer < _duration)
        {
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                float _distance = Vector3.Distance(_player.transform.position, enemy.transform.position);
                float _slowdown = !_opposite
                    ? Mathf.Clamp(_maxSlowDown - (_distance * 10f), 0f, _maxSlowDown)
                    : Mathf.Clamp(_distance * 10f, 0f, _maxSlowDown);

                enemy.MassSlowDown(_slowdown);
            }
            if (!_permanent)
            {
                _timer += 0.1f;
            }
            yield return new WaitForSeconds(0.1f);
        }
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.Normal();
        }
        _player.Normal();
    }

}
