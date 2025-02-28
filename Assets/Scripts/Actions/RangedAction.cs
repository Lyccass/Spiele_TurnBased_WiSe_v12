using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RangedAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooldown,
    }

    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private int maxRangedDistance = 7;
    [SerializeField] private int minDamage = 3;
    [SerializeField] private int maxDamage = 6;

    private float stateTimer;
    private State state;
    private Unit targetUnit;
    private bool hasShot;  // Statt `canShootBullet`

    protected override void Awake()
    {
        base.Awake();
        _priorityMultiplier = 2.0f; // Höhere Priorität für das Schießen
    }

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                float rotateSpeed = 10f;
                UnityEngine.Vector3 aimDir = (targetUnit.GetWordPosition() - unit.GetWordPosition()).normalized;
                UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(aimDir);
                transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

                break;

            case State.Shooting:
                if (!hasShot)
                {
                    hasShot = true;  // Verhindert mehrfaches Schießen
                    Invoke(nameof(Shoot), 0.8f);  // Verzögerung beim Schießen
                }
                break;

            case State.Cooldown:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = 0.5f; // Kürzere Verzögerung für besseres Feedback
                break;

            case State.Shooting:
                state = State.Cooldown;
                stateTimer = 0.5f; // Kürzere Cooldown-Zeit
                break;

            case State.Cooldown:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        int randomDamage = UnityEngine.Random.Range(minDamage, maxDamage);
        targetUnit.Damage(randomDamage);
    }

    public override string GetActionName()
    {
        return "Ranged";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxRangedDistance; x <= maxRangedDistance; x++)
        {
            for (int z = -maxRangedDistance; z <= maxRangedDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxRangedDistance) continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                UnityEngine.Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPositionn(unitGridPosition);
                UnityEngine.Vector3 rangedDir = (targetUnit.GetWordPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;

                if (Physics.Raycast(
                        unitWorldPosition + UnityEngine.Vector3.up * unitShoulderHeight,
                        rangedDir,
                        UnityEngine.Vector3.Distance(unitWorldPosition, targetUnit.GetWordPosition()),
                        obstacleLayerMask))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        AudioManager.Instance.PlaySFX("RangedAttack");
        stateTimer = 1f;
        hasShot = false;  // Rücksetzen, damit der Schuss erst im richtigen Moment passiert

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxRangeDistance()
    {
        return maxRangedDistance;
    }

    public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        if (targetUnit == null) return null;

        float actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNomalized()) * 100f);

        UnityEngine.Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPositionn(gridPosition);
        UnityEngine.Vector3 targetWorldPosition = targetUnit.GetWordPosition();
        UnityEngine.Vector3 rangedDir = (targetWorldPosition - unitWorldPosition).normalized;
        float unitShoulderHeight = 1.7f;

        if (Physics.Raycast(
                unitWorldPosition + UnityEngine.Vector3.up * unitShoulderHeight,
                rangedDir,
                UnityEngine.Vector3.Distance(unitWorldPosition, targetWorldPosition),
                obstacleLayerMask))
        {
            actionValue = 0;
        }

        return new EnemyAIAction(gridPosition, actionValue);
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionList(gridPosition).Count;
    }
}
