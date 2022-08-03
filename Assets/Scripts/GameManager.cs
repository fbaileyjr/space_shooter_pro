using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && _isGameOver)
        {
            Application.Quit();
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    void RestartLevel() //Restarts the level
    {
        SceneManager.LoadScene(1); // Current Game Scene
    }
}
