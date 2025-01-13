using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    [SerializeField] float fireballRadius = 4f;
    [SerializeField] private Transform fireballPrefab;
    [SerializeField] private TrailRenderer trailRenderer;

    private Vector3 targetPosition;
    private Action onFireballBehaviourComplete;
    private Unit activeUnit; // Reference to the unit that activated the ability

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, fireballRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    // Exclude the active unit from taking damage
                    if (targetUnit != activeUnit)
                    {
                        targetUnit.Damage(UnityEngine.Random.Range(1, 3));
                    }
                }

                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
                }
            }

            trailRenderer.transform.parent = null;
            Instantiate(fireballPrefab, targetPosition + Vector3.up * 1.2f, Quaternion.identity);
            Destroy(gameObject);
            onFireballBehaviourComplete?.Invoke();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onFireballBehaviourComplete, Unit activeUnit)
    {
        this.onFireballBehaviourComplete = onFireballBehaviourComplete;
        this.activeUnit = activeUnit; // Store the reference to the active unit
        targetPosition = LevelGrid.Instance.GetWorldPositionn(targetGridPosition);
    }
}
