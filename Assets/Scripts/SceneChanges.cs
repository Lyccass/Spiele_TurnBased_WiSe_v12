using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Checks if the scene exists in the build settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene not found: " + sceneName);
        }
    }

    
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void LoadMap()
    {
        LoadScene("Map");
    }

    public void LoadHub()
    {
        LoadScene("Hub");
    }

      public void LoadLevel(string levelName)
    {   
        LoadScene(levelName);
    }

      //reload current scene
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LoadScene(currentScene);
        
    }

        public void ExitGame()
    {
        Application.Quit();
    }
}
