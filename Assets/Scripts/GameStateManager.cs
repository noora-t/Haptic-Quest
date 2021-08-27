using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject _diamond;
    [SerializeField]
    GameObject _mainCamera;
    [SerializeField]
    GameObject _player;

    UIController _uiController;
    GameControls _gameControls;

    List<GameObject> _diamondList = new List<GameObject>();

    void Start()
    {
        _gameControls = FindObjectOfType<GameControls>();
        _uiController = GetComponentInParent<UIController>();

        Time.timeScale = 1;

        _gameControls.ShowCursor(false);

        // Hide menus that are not needed
        HidePauseMenu();
        _uiController.HandleMenus(false, "endingObjects");

        // Instantiate diamonds to the game area and add them to a list
        _diamondList.Add(Instantiate(_diamond, new Vector3(28, 23, 56), Quaternion.identity));
        _diamondList.Add(Instantiate(_diamond, new Vector3(91, 23, 78), Quaternion.identity));
        _diamondList.Add(Instantiate(_diamond, new Vector3(71, 23, -2), Quaternion.identity));
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 1)               
                ShowPauseMenu();               
            else if (Time.timeScale == 0)
                HidePauseMenu();
        }
    }

    public List<GameObject> GetDiamondList()
    {
        return _diamondList;
    }

    public void RemoveFromDiamondList(GameObject removable)
    {
        _diamondList.Remove(removable);
    }

    public void Reload()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPauseMenu()
    {
        _gameControls.ShowCursor(true);
        _mainCamera.GetComponent<CameraController>().FreezeCamera(true);
        _player.GetComponent<PlayerMovement>().SetFreeze(true);
        Time.timeScale = 0;
        _uiController.HandleMenus(true, "pauseObjects");
    }

    public void HidePauseMenu()
    {
        _gameControls.ShowCursor(false);
        _mainCamera.GetComponent<CameraController>().FreezeCamera(false);
        _player.GetComponent<PlayerMovement>().SetFreeze(false);
        Time.timeScale = 1;
        _uiController.HandleMenus(false, "pauseObjects");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
