using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform arrowPrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake() 
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<RangedAction>(out RangedAction rangedAction))
        {
            rangedAction.OnShoot += RangedAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isWalking", false);
    }

    private void RangedAction_OnShoot(object sender, RangedAction.OnShootEventArgs e)
    {
        animator.SetTrigger("shoot");
        Transform bulletProjectileTransform = 
            Instantiate(arrowPrefab, shootPointTransform.position, Quaternion.identity);

        Projectile projectile = bulletProjectileTransform.GetComponent<Projectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWordPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y+1;
        projectile.Setup(targetUnitShootAtPosition);
    }
}
