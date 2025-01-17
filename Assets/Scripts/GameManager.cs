using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Elimination, // Win by killing all enemies
    Hunt, // Win by opening chest
    Assasination //Win by killing boss enemy

}

public class GameManager : MonoBehaviour
{    
    public static GameManager Instance { get; set; }
    private bool gameEnded = false;
    public GameState currentGameState = GameState.Playing;
    [SerializeField] private GameMode currentGameMode;

    
    private void Awake()
    {
        // Listen for when scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        Time.timeScale = 1f; //unfreeze time on scene load
    }

    void Update()
    {
         CheckWinLoseConditions();
    }

    void CheckWinLoseConditions()
    {
        if (gameEnded) return;

        switch (currentGameMode)
        {
            case GameMode.Elimination:
            CheckEliminationConditions();
            break;

            //case GameMode.Hunt:
            //CheckHuntConditions();
            //break;

            case GameMode.Assasination:
            CheckAssasinationConditions();
            break;
        } 
    }

    void CheckEliminationConditions()
    {
        
        var enemyUnits = UnitManager.Instance.GetEnemyUnitList();
        if (enemyUnits.Count == 0)
            {
            GameOver("Win");
            }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
            {
            GameOver("Lose");
            }
       
            
    }

    /*void CheckHuntConditions()
    {
        if (ChestFound())
        {
            GameOver("Win");
        }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
                {
                GameOver("Lose");
                }
    }
    */
     void CheckAssasinationConditions()
    {
        if (BossDefeated())
        {
            GameOver("Win");
        }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
                {
                GameOver("Lose");
                }
    }


    /*private bool ChestFound()
    {
        GameObject chest = GameObject.FindGameObjectWithTag("Chest"); // object with chest tag in scene
        return chest != null && chest.IsOpened(); //chest. whatever method you have for interacting with objects
    }
    */
    bool BossDefeated()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss"); //enemy tag with boss
        return boss == null;
    }


     void GameOver(string result)
    {
        gameEnded = true;
        currentGameState = GameState.GameOver;
        Debug.Log("Game Over! Result: " + result);
        Time.timeScale = 0f; // Freeze game time

         // Show a win/lose screen
        GameManagerUI.Instance.ShowGameOverScreen(result);
       
    }

        public GameState GetCurrentGameState()
    {
        return currentGameState;
    }
}
