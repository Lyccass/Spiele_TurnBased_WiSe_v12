using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance{get; private set;}  
    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;
    private Unit bossUnit;
    


    private void Awake() 
    {  
        if(Instance != null){
        Debug.LogError("Error, more than one UnitManager");
        Destroy(gameObject);
        return;
    }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }
    private void Start() 
    {
        Unit.OnAnyUnitSpawned +=  Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead +=  Unit_OnAnyUnitDead;
    }

      private void OnDestroy()
    {
        // Unsubscribe from events to avoid duplicate calls
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Debug.Log(unit + " Unit spawned");
        unitList.Add(unit);

        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
            if (unit.CompareTag("Boss")){
                bossUnit = unit;
                Debug.Log("Boss spawned!");
            }
        }
        else
        {
            friendlyUnitList.Add(unit);
        }
        
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Debug.Log(unit + " Unit died");
        unitList.Remove(unit);

        if(unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
            GameManager.Instance.enemies --;
            GameManager.Instance.killedEnemies ++;
            Debug.Log($"Enemy killed. Remaining enemies: {GameManager.Instance.enemies}");
            if(GameManager.Instance.currentGameMode == GameMode.Assasination && unit == bossUnit)
                {
                    GameManager.Instance.boss --;
                    Debug.Log($"Boss defeated! Remaining bosses: {GameManager.Instance.boss}");
                }
            
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
        
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }


}
