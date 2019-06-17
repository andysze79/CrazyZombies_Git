using System.Collections.Generic;
using UnityEngine;

public class State_Attack_Ranged : State_Attack 
{
    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================  

    private Vector3 projectileSpawnPositionOffset
    {
        get
        {
            return transform.position
            + (transform.forward * Data.projectileSpawnOffset.z)
            + (transform.right * Data.projectileSpawnOffset.x)
            + (transform.up * Data.projectileSpawnOffset.y);
        }
    }

    public void ShootProjectile(GameObject _go)
    {
        if (Data.enemy == null) return;

        if (Data.arrowPrefab == null) return;
        {
            ProjectileBase projectile = Instantiate(Data.arrowPrefab, projectileSpawnPositionOffset, Quaternion.identity).GetComponent<ProjectileBase>();
            if (!Data.homingProjectile) projectile.SetTarget(Data.enemy.transform.position);
            if (Data.homingProjectile) projectile.SetTarget(Data.enemy.gameObject);
            projectile.SetProjectileSpeedInSeconds(Data.projectileSpeedInSeconds);
            projectile.SetProjectileTrajectoryHeight(Data.projectileTrajectoryHeight);
            projectile.SetProjectileTargetOffsetY(Data.projectileTargetOffsetY);
            projectile.SetDamagableLayerMask(Data.searchLayerMask);
            projectile.SetProjectileCasterAndReceiver(Data, Data.enemy);
        }
    }

    protected Vector3 Curve(Vector3 initialPos, Vector3 targetPos, float t)
    {
        float durationRef = t / Data.projectileSpeedInSeconds;
        Vector3 nPos = Vector3.zero;

        if (t <= Data.projectileSpeedInSeconds)
        {
            nPos = Vector3.Lerp(initialPos, targetPos, durationRef);
            nPos.y += (1 - Mathf.Pow(2 * durationRef - 1, 2)) * Data.projectileTrajectoryHeight;
        }
        else
        {
            nPos = targetPos;
        }

        return nPos;
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        if (Owner.ActiveState != AIStateKeeper.States.Attack) return;

        Gizmos.DrawWireSphere(projectileSpawnPositionOffset, 0.25f);

        if (Data.enemy == null) return;

        Gizmos.color = Color.red;

        List<Vector3> allCurvePos = new List<Vector3>();

        for (int j = 0; j < Data.projectileSpeedInSeconds * 100; j++)
        {
            Vector3 pos = Curve(projectileSpawnPositionOffset, new Vector3(Data.enemy.transform.position.x, Data.enemy.transform.position.y + Data.projectileTargetOffsetY, Data.enemy.transform.position.z), (float)j / 50);
            allCurvePos.Add(pos);
        }

        for (int j = 0; j < allCurvePos.Count; j++)
        {
            int prev = 0;
            if (j - 1 < 0)
            {
                prev = 0;
            }
            else
            {
                prev = j - 1;
            }

            Gizmos.DrawLine(allCurvePos[prev], allCurvePos[j]);
        }
    } 
}