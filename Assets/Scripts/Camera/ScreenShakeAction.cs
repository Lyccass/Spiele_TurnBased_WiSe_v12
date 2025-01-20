using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    private void Start() 
    {
        RangedAction.OnAnyShoot += RangedAction_OnAnyShoot;
        FireBallProjectile.OnAnyGrenadeExploded += FireBallProjectile_OnAnyGrenadeExploded;
    }

    private void FireBallProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(8f);
    }

    private void RangedAction_OnAnyShoot(object sender, RangedAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(3f);
    }
}
