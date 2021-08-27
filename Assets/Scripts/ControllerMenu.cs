using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField]
    Button _option1;
    [SerializeField]
    Button _option2;
    [SerializeField]
    Button _option3;

    [SerializeField]
    GameObject _scripts;

    const int NUMBEROFOPTIONS = 3;

    int _selectedOption;

    bool _axisIsInUse = false;

    Text _option1ButtonText;
    Text _option2ButtonText;
    Text _option3ButtonText;

    Color32 _normalButtonColor = new Color32(255, 255, 255, 255);
    Color32 _higlightedButtonColor = new Color32(0, 215, 241, 255);

    Color32 _normalButtonTextColor = new Color32(0, 0, 0, 255);
    Color32 _highlightedButtonTextColor = new Color32(255, 255, 255, 255);

    GameControls _gameControls;

    public void HighlightFirstOption()
    {
        // Highlight the first option in the menu
        _selectedOption = 1;
        _option1.image.color = _higlightedButtonColor;
        _option1ButtonText.color = _highlightedButtonTextColor; 
    }

    void Start()
    {
        // Get all menu button texts
        _option1ButtonText = _option1.GetComponentInChildren<Text>();
        _option2ButtonText = _option2.GetComponentInChildren<Text>();
        _option3ButtonText = _option3.GetComponentInChildren<Text>();

        _gameControls = FindObjectOfType<GameControls>();

        if (_gameControls.IsUsingController)
            HighlightFirstOption();
    }

    void Update()
    {      
        if (Input.GetAxisRaw("7th") != 0) // Controller is used to browse menu
        {
            if (_axisIsInUse == false)
            {
                // Check where the user is going in the menu
                HandleSelectedOption();
                
                // Make sure all other buttons will be normal color
                SetButtonsToNormal();
                
                // Set the visual indicator for which option player is on
                SetVisualIndicator();

                _axisIsInUse = true;
            }
        }
        else
        {
            _axisIsInUse = false;
        }

        if (Input.GetButtonDown("Submit")) // An option from the menu is selected
        {
            if (gameObject.name.Equals("MainMenuCanvas")) // Main menu
                HandleMainMenuCanvas();
                
            else if (gameObject.name.Equals("PauseMenuCanvas")) // Pause menu
                HandlePauseMenuCanvas();
                
            else if (gameObject.name.Equals("GameEndingCanvas")) // Ending menu
                HandleGameEndingCanvas();               
        }
    }

    private void HandleSelectedOption()
    {
        if (Input.GetAxisRaw("7th") < 0) // Input telling to go down
        {
            _selectedOption += 1;
            if (_selectedOption > NUMBEROFOPTIONS) // If at end of list go back to top
                _selectedOption = 1;
        }
        else if (Input.GetAxisRaw("7th") > 0) // Input telling to go up
        {
            _selectedOption -= 1;
            if (_selectedOption < 1) // If at top of list go to end of list
                _selectedOption = NUMBEROFOPTIONS;
        }
    }

    public void SetButtonsToNormal()
    {
        _option1.image.color = _normalButtonColor;
        _option2.image.color = _normalButtonColor;
        _option3.image.color = _normalButtonColor;

        _option1ButtonText.color = _normalButtonTextColor;
        _option2ButtonText.color = _normalButtonTextColor;
        _option3ButtonText.color = _normalButtonTextColor;
    }

    private void SetVisualIndicator()
    {
        if (_selectedOption == 1)
        {
            _option1.image.color = _higlightedButtonColor;
            _option1ButtonText.color = _highlightedButtonTextColor;
        }
        else if (_selectedOption == 2)
        {
            _option2.image.color = _higlightedButtonColor;
            _option2ButtonText.color = _highlightedButtonTextColor;
        }
        else if (_selectedOption == 3)
        {
            _option3.image.color = _higlightedButtonColor;
            _option3ButtonText.color = _highlightedButtonTextColor;
        }
    }

    private void HandleMainMenuCanvas()
    {
        switch (_selectedOption)
        {
            case 1:
                _scripts.GetComponent<MainMenu>().StartGameplay();
                break;
            case 2:
                _scripts.GetComponent<MainMenu>().ShowHelp();
                break;
            case 3:
                Application.Quit();
                break;
        }
    }

    private void HandlePauseMenuCanvas()
    {
        switch (_selectedOption)
        {
            case 1:
                _scripts.GetComponent<GameStateManager>().HidePauseMenu();
                break;
            case 2:
                _scripts.GetComponent<GameStateManager>().Reload();
                break;
            case 3:
                _scripts.GetComponent<GameStateManager>().LoadMainMenu();
                break;
        }
    }

    private void HandleGameEndingCanvas()
    {
        switch (_selectedOption)
        {
            case 1:
                _scripts.GetComponent<GameStateManager>().Reload();
                break;
            case 2:
                _scripts.GetComponent<GameStateManager>().LoadMainMenu();
                break;
            case 3:
                Application.Quit();
                break;
        }
    }
}
