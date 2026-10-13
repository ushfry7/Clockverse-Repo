using System.Collections;
using UnityEngine;

public class OmitTempo : TempoSystem
{
    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                JoyOmitSelected();
                break;
            case 2:
                HexOmitSelected();
                break;
            case 3:
                MixOmitSelected();
                break;
            case 4:
                SinOmitSelected();
                break;
            default:
                BasicOmitSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1:
                Joy_Omit();
                break;
            case 2:
                Hex_Omit();
                break;
            case 3:
                Mix_Omit();
                break;
            case 4:
                Sin_Omit();
                break;
            default:
                Basic_Omit();
                break;
        }
    }

    void BasicOmitSelected()
    {
        // ================= OMIT =================

        // Base Omit
        // Duration: Up to 10s
        // Recharge: 10s
        // Operation:
        // Skip forward through time for up to 10s.
        // Player can move and act during the skipped time.
        // Can be ended early.
        // After the effect ends, normal time resumes.
        TempoValues(20f, 10f, 1, 0, false);
    }

    void Basic_Omit()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.Freeze(_tempoEffectDuration);
            }
        }
    }

    public void JoyOmitSelected()
    {
        // Joy
        // Duration: 5s
        // Recharge: 10s
        // Operation:
        // Reduce Omit duration to 5s.
        // Cooldown recovers 3x faster while standing still.
        TempoValues(15f, 5f, 1, 0, false);
    }

    void Joy_Omit()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            StartCoroutine(Joy_seqyence());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.Freeze(_tempoEffectDuration);
            }
        }
    }

    IEnumerator Joy_seqyence()
    {
        while (_currentCoolDownTimer > 0)
        {
            if (!_player.HaveForce())
            {
                _decreaseAmount = 3;
            }
            else
            {
                _decreaseAmount = 1;
            }
            yield return new WaitForSeconds(0.1f);
        }
        _decreaseAmount = 1;
    }

    public void HexOmitSelected()
    {
        // Hex
        // Duration: Up to 10s + 5s after deactivation
        // Recharge: 10s
        // Operation:
        // After Omit ends, enemies cannot target the player for 5s.
        // Player can still attack enemies.
        // Enemies can still damage the player.
        TempoValues(15f, 5f, 1, 0, true);
    }

    void Hex_Omit()
    {
        if (_charges > 0)
        {
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.Freeze(_tempoEffectDuration, _tempoEffectDuration);
            }
        }
    }

    public void MixOmitSelected()
    {
        // Mix
        // Duration: 7.5s automatic / 2s manual
        // Recharge: 10s
        // Operation:
        // Taking damage automatically activates Omit.
        // During the skip, player's time speed becomes 500%.
        // Manual activation lasts only 2s.
        TempoValues(10f, 2f, 1, 0, false);
        StartCoroutine(Mix_Sequence());
    }

    void Mix_Omit()
    {
        if (_charges > 0)
        {
            _tempoCooldown = 12f;
            _tempoEffectDuration = 2f;
            StartCoroutine(CoolDownPeriod());
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.Freeze(_tempoEffectDuration);
                _player.ZionTempo(_tempoEffectDuration, 5f, 1f);
            }
        }
    }

    IEnumerator Mix_Sequence()
    {
        while (true)
        {
            if (_player._tookHit)
            {
                if (_charges > 0)
                {
                    _tempoCooldown = 17.5f;
                    _tempoEffectDuration = 7.5f;
                    StartCoroutine(CoolDownPeriod());
                    foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
                    {
                        enemy.Freeze(_tempoEffectDuration);
                    }
                    _player.ZionTempo(_tempoEffectDuration, 5f, 1f);
                    _player._tookHit = false;
                }
                else
                {
                    _player._tookHit = false;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SinOmitSelected()
    {
        // Sin
        // Duration: Up to 10s
        // Recharge: Starts at 2s
        // Operation:
        // Cooldown starts at 2s.
        // Each use adds 2s to the cooldown.
        // The added cooldown penalty is halved when entering a new stage.
        TempoValues(4f, 2f, 1, 0, false);
    }

    void Sin_Omit()
    {
        if (_charges > 0)
        {
            if (_gameUIManager.StageChange())
            {
                _tempoCooldown /= 2f;
            }
            StartCoroutine(CoolDownPeriod());
            _tempoCooldown += 2f;
            foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
            {
                enemy.Freeze(_tempoEffectDuration);
            }
        }
    }
}
