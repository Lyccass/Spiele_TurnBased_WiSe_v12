using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAction
{
    public GridPosition gridPosition;
    public float actionValue;

    public EnemyAIAction(GridPosition gridPosition, float actionValue)
    {
        this.gridPosition = gridPosition;
        this.actionValue = actionValue;
    }
}
