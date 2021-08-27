using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameControls _gameControls;

    GameObject[] _helpObjects;

    public Image controller;
    public Image mouse;

    void Start()
    { 
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _gameControls = FindObjectOfType<GameControls>();

        _helpObjects = GameObject.FindGameObjectsWithTag("ShowOnHelp");

        HideHelp();
    }

    public void StartGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Close"))
            HideHelp();

        if (Input.GetButtonDown("ChangeControls"))
        {
            _gameControls.ChangeControls();

            if (mouse.enabled == true)
            {
                mouse.enabled = false;
                controller.enabled = true;
            }
            else if (controller.enabled == true)
            {
                controller.enabled = false;
                mouse.enabled = true;
            }
            
        }
           
                    
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowHelp()
    {
        foreach (GameObject g in _helpObjects)
            g.SetActive(true);       
    }

    public void HideHelp()
    {
        foreach (GameObject g in _helpObjects)
            g.SetActive(false);
    }

    public void SwitchControls()
    {
        _gameControls.ChangeControls();
    }
}
