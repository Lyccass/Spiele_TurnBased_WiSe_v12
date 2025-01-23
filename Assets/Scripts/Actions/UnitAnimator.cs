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
    [SerializeField] private AnimatorOverrideController characterAnimatorOverride; // Override for specific character animations

    private void Awake() 
    {   
         // Set character-specific animations if an override is provided
        if (characterAnimatorOverride != null)
        {
            animator.runtimeAnimatorController = characterAnimatorOverride;
        }

        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<RangedAction>(out RangedAction rangedAction))
        {
            rangedAction.OnShoot += RangedAction_OnShoot;
        }

        if (TryGetComponent<MeeleAction>(out MeeleAction meeleAction))
        {
            meeleAction.OnMeleeAttack += MeeleAction_OnMeleeAttack;
        }

        if (TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnDead += HealthSystem_OnDead; 
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

      private void MeeleAction_OnMeleeAttack(object sender, EventArgs e)
    {
        animator.SetTrigger("attack");
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        animator.SetTrigger("hit");
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        animator.SetTrigger("die");
    }

}
