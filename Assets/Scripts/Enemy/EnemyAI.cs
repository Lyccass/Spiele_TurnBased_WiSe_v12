using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private float timer;
    private State state;
    private int currentEnemyIndex;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // If no more actions can be taken, move to the next enemy unit
                        currentEnemyIndex++;
                        if (currentEnemyIndex >= UnitManager.Instance.GetEnemyUnitList().Count)
                        {
                            // All enemy units have taken their turn
                            state = State.WaitingForEnemyTurn;
                            TurnSystem.Instance.NextTurn();
                        }
                        else
                        {
                            // Reset timer for the next unit
                            timer = 0.2f;
                        }
                    }
                }
                break;

            case State.Busy:
                // Wait for the action to complete
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.2f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            timer = 1.5f;
            state = State.TakingTurn;
            currentEnemyIndex = 0; // Reset index when a new enemy turn starts
        }
    }

    private bool TryTakeEnemyAIAction(Action OnEnemyAIActionComplete)
    {
        if (currentEnemyIndex < UnitManager.Instance.GetEnemyUnitList().Count)
        {
            Unit enemyUnit = UnitManager.Instance.GetEnemyUnitList()[currentEnemyIndex];
            return TryTakeEnemyAIAction(enemyUnit, OnEnemyAIActionComplete);
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action OnEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }

        // Check if action points can be spent
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            Debug.Log("Not enough action points!");
            return false;
        }

        Debug.Log("Spin Action taken!");
        spinAction.TakeAction(actionGridPosition, OnEnemyAIActionComplete);
        return true;
    }
}
