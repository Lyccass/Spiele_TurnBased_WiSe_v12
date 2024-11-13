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
    EnemyAIAction bestEnemyAIAction = null;
    BaseAction bestBaseAction = null;

    foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
    {
        if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
        {
            continue;
        }

        EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
        if (testEnemyAIAction != null)
        {
            if (bestEnemyAIAction == null || testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
            {
                bestEnemyAIAction = testEnemyAIAction;
                bestBaseAction = baseAction;
            }
        }
    }

    // Even if the best action doesn't get us all the way to the player, take it if it brings us closer
    if (bestEnemyAIAction != null)
    {
        // Attempt to spend action points and execute the action
        if (enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, OnEnemyAIActionComplete);
            return true;
        }
    }

    return false;
}
}