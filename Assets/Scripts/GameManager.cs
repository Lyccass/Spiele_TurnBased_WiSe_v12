using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{    
    public static GameManager Instance { get; set; }
    private bool gameEnded = false;
    public GameState currentGameState = GameState.Playing;

    
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

    public void Update()
    {
         CheckWinLoseConditions();
    }

    void CheckWinLoseConditions()
    {
        if (gameEnded) return;

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        var enemyUnits = UnitManager.Instance.GetEnemyUnitList();

            if (friendlyUnits.Count == 0)
                {
                GameOver("Lose");
                }
       
            else if (enemyUnits.Count == 0)
                {
                GameOver("Win");
                }
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
