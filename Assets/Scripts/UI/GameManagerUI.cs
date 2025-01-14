using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;

    public void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        Instance = this; // Set the instance
    }

    public void Start()
    {
         gameOverPanel.SetActive(false);
    }

    public void ShowGameOverScreen(string result)
    {
        gameOverPanel.SetActive(true);

        TextMeshProUGUI gameOverTitle = gameOverPanel.transform.Find("GameOverPanel").GetComponent<TextMeshProUGUI>();
        gameOverTitle.text = result == "Win" ? "Sucess!" : "Fail";
    }

     public void backToMap()
    {
        // change scene to map
    }

       public void retry()
    {
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
