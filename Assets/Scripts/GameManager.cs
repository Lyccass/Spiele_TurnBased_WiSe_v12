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
    private bool isPaused = false;
    public GameState currentGameState = GameState.Playing;
    [SerializeField] public GameMode currentGameMode;
    [SerializeField] public int enemies;
    public int killedEnemies = 0;
    [SerializeField] public int boss;
    [SerializeField] public int chest;
    public UnitManager unitManager;

    
    private void Awake()
    {   
         if (Instance != null)
        {
            Debug.LogError("Error, more than one GameManager instance in the scene.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Listen for when scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void Start()
    {
        unitManager = GetComponent<UnitManager>();
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

        switch (currentGameMode)
        {
            case GameMode.Elimination:
            CheckEliminationConditions();
            break;

            case GameMode.Hunt:
            CheckHuntConditions();
            break;

            case GameMode.Assasination:
            CheckAssasinationConditions();
            break;
        } 
    }

    void CheckEliminationConditions()
    {

        if (enemies == 0)
            {
            GameOver("Win");
            }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
            {
            GameOver("Lose");
            }
             
    }

    void CheckHuntConditions()
    {
        if (chest == 0)
        {
            GameOver("Win");
        }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
                {
                GameOver("Lose");
                }
    }
    

     void CheckAssasinationConditions()
    {
        if (boss == 0)
        {
            GameOver("Win");
        }

        var friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count == 0)
                {
                GameOver("Lose");
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

    public void PauseGame()
    {
        isPaused = true;
        currentGameState = GameState.Pause;
        Debug.Log("Game Paused.");
        Time.timeScale = 0f;

        GameManagerUI.Instance.ShowPausedScreen();
    }

    public void ResumeGame()
    {
        isPaused = false;
        currentGameState = GameState.Playing;
        Debug.Log("Game Resumed.");
        Time.timeScale = 1f;

        GameManagerUI.Instance.HidePausedScreen();
    }



        public GameState GetCurrentGameState()
    {
        return currentGameState;
    }


}


