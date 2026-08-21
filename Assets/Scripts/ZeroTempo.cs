using UnityEngine;

public class ZeroTempo : TempoSystem
{
    void Basic_Zero()
    {
        //        Reset all cooldowns
        //Recharge: 15s.
        _tempoCooldown = 15f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 1f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 1;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

    void Zen_Zero()
    {
        //        Zero + Zen

        //Zen = Tranquility

        //Instead of completely resetting cooldowns:

        //        Reduce cooldowns by 50 % of their maximum cooldown.

        //And Zen recharges 2× faster.
        _tempoCooldown = 15f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 1f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 1;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

    void Cry_Zero()
    {
        //        Zero + Cry

        //Zero becomes passive.

        //Player cooldowns are passively reduced by 25 %.

        //So you don't press Zero.
        _tempoCooldown = 15f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 1f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 1;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

    void Vex_Zero()
    {
        //        Zero + Vex
        //4 extra charges
        //Each charge has a 50s cooldown.

        //So you have a lot of Zero uses, but you need to manage them because recharge is extremely long.
        _tempoCooldown = 50f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 1f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 4;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

    void Age_Zero()
    {
        //        Zero + Age

        //Age = Persistence

        //Instead of Zero being an instant effect:

        //        ZERO
        //↓
        //effect happens over 3 seconds

        //Cooldown becomes 22s.
        _tempoCooldown = 22f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 3f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 1;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

    void Sap_Zero()
    {
        //        Zero + Sap

        //The GDD lists Sap as a challenge / weakness - style Epoch.

        //For Zero:

        //It works the same way, but all enemies also have their cooldowns reduced by 50 %.
        _tempoCooldown = 22f;
        _currentCoolDownTimer = _tempoCooldown;
        _tempoEffectDuration = 3f;
        _effectPenality = 0;
        _effectPenalityResetDuration = 0;
        _autoCall = false;
        _chargeLimit = 1;
        _timeToActivate = 0f;
        _canCancelEarly = false;
    }

}
