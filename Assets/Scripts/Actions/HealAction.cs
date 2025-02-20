using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : BaseAction
{
    [SerializeField] private int maxHealDistance = 5; // Healing range
    [SerializeField] private int minHealAmount = 3; // Minimum healing amount
    [SerializeField] private int maxHealAmount = 6; // Maximum healing amount

    private Unit targetUnit;

    private enum State
    {
        Aiming,
        Healing,
        Cooldown
    }

    private State state;
    private float stateTimer;

    protected override void Awake()
    {
        base.Awake();
        _priorityMultiplier = 1.5f; // Priority for healing action
    }
private bool hasHealed; // Flag to prevent multiple heal executions

private void Update()
{
    if (!isActive) return;

    stateTimer -= Time.deltaTime;

    switch (state)
    {
        case State.Aiming:
            // Rotate towards target unit
            float rotateSpeed = 10f;
            Vector3 aimDir = (targetUnit.GetWordPosition() - unit.GetWordPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
            break;

        case State.Healing:
            if (!hasHealed)
            {
                Heal();
                hasHealed = true; // Ensure Heal() is called only once
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
            state = State.Healing;
            stateTimer = 0.5f; // Healing animation delay
            hasHealed = false; // Reset the flag for the next action
            break;

        case State.Healing:
            state = State.Cooldown;
            stateTimer = 0.5f; // Cooldown before completing action
            break;

        case State.Cooldown:
            ActionComplete();
            break;
    }
}

private void Heal()
{
    targetUnit.GetComponent<HealthSystem>().Heal(UnityEngine.Random.Range(minHealAmount, maxHealAmount));
    Debug.Log($"{unit.name} healed {targetUnit.name}.");
}
    public override string GetActionName()
    {
        return "Heal";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxHealDistance; x <= maxHealDistance; x++)
        {
            for (int z = -maxHealDistance; z <= maxHealDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                // Ensure distance is within range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxHealDistance)
                {
                    continue;
                }

                // Check for unit presence
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
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
        stateTimer = 1f; // Time spent aiming

        ActionStart(onActionComplete);
    }

     public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
    {
       return new EnemyAIAction(gridPosition,0f);
}
}
