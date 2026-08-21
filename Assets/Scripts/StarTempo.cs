using UnityEngine;

public class StarTempo : TempoSystem
{

    public override void EpochSelect()
    {
        base.EpochSelect();
        switch (_epochSelected)
        {
            case 1:
                RueStarSelected();
                break;
            case 2:
                FoeStarSelected();
                break;
            case 3:
                AntStarSelected();
                break;
            case 4:
                PayStarSelected();
                break;
            default:
                BasicStarSelected();
                break;
        }
    }

    public override void EpochCall()
    {
        base.EpochCall();
        switch (_epochSelected)
        {
            case 1: Rue_Star();
                break;
            case 2: Foe_Star();
                break;
            case 3: Ant_Star();
                break;
            case 4: Pay_Star();
                break;
            default: Basic_Star();
                break;
        }
    }

    public void BasicStarSelected()
    {
        //Base — Star
        //Effect:
        //Instantly stop time for enemies.
        //Enemy time = stopped
        //Duration = 2s
        //Cooldown = 8s
        //No wind - up.
        TempoValues(8f, 2f, 1, 0, false);
    }

    void Basic_Star()
    {
        StartCoroutine(CoolDownPeriod());
        FreezeEnemyTime();
    }

    public void RueStarSelected()
    {
        //Rue:
        //3s cooldown
        //But every activation decreases the freeze duration by 0.5s.
        //The penalty remains until Star hasn't been activated for 12 seconds.
        TempoValues(3f, 2f, 1, 0, false);
    }

    void Rue_Star()
    {
        if(_tempoEffectDuration == 2f)
        {
            _tempoCooldown = 3f;
            StartCoroutine(CoolDownPeriod());
            FreezeEnemyTime();
            _tempoEffectDuration -= 0.5f;
        }
        else if(_tempoEffectDuration > 0.5f)
        {
            StartCoroutine(CoolDownPeriod());
            FreezeEnemyTime();
            _tempoEffectDuration -= 0.5f;
        }
        else if(_tempoEffectDuration == 0.5f)
        {
            StartCoroutine(CoolDownPeriod());
            FreezeEnemyTime();
            _tempoEffectDuration = 2f;
            _tempoCooldown = 12f;
        }
    }

    public void FoeStarSelected()
    {
        //Foe = Resentment
        //Instead of freezing everyone:
        //Star only affects the enemy closest to your cursor.
        TempoValues(8f, 3f, 1, 0, false);
    }

    void Foe_Star()
    {
        Vector3 screenCenter = new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            0f
        );

        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        Collider[] enemies = Physics.OverlapSphere(
            ray.origin + ray.direction * (30f * 0.5f),
            30f * 0.5f
        );

        BaseEnemyAI closestEnemy = null;
        float closestScreenDistance = float.MaxValue;

        foreach (Collider col in enemies)
        {
            BaseEnemyAI enemy = col.GetComponentInParent<BaseEnemyAI>();

            if (enemy == null)
                continue;

            Vector3 viewportPosition =
                Camera.main.WorldToViewportPoint(enemy.transform.position);
            if (viewportPosition.z <= 0f)
                continue;
            float screenDistance = Vector2.Distance(
                new Vector2(viewportPosition.x, viewportPosition.y),
                new Vector2(0.5f, 0.5f)
            );

            if (screenDistance < closestScreenDistance)
            {
                closestScreenDistance = screenDistance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.Freeze(_tempoEffectDuration);
        }
    }

    public void AntStarSelected()
    {
        //Ant = Insignificance
        //Everything is weaker:
        //2s → 1s freeze
        //8s → 4s cooldown
        TempoValues(4f, 1f, 1, 0, false);
    }

    void Ant_Star()
    {
        StartCoroutine(CoolDownPeriod());
        FreezeEnemyTime();
    }

    public void PayStarSelected()
    {
        //Pay = Vengeance

        //Instead:
        //TAKE DAMAGE
        //     ↓
        //+25 % STAR CHARGE
        //And:
        //DEAL 1 DAMAGE
        //     ↓
        //+2 % STAR CHARGE
        //You cannot recharge Star while Star is currently active.
        TempoValues(0, 2f, 1, 0, false);
    }

    void Pay_Star()
    {
        if(_currentRecharge >= 100)
        {
            FreezeEnemyTime();
        }
    }

    void FreezeEnemyTime()
    {
        foreach (BaseEnemyAI enemy in FindObjectsByType<BaseEnemyAI>(FindObjectsSortMode.None))
        {
            enemy.Freeze(_tempoEffectDuration);
        }
    }
}
