using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    ControllerMenu _controllerMenu;
    
    bool _isUsingController = false;

    public bool IsUsingController => _isUsingController;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _controllerMenu = FindObjectOfType<ControllerMenu>();
    }

    public void ChangeControls()
    {
        _isUsingController = !_isUsingController;

        if (_isUsingController)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _controllerMenu.HighlightFirstOption();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _controllerMenu.SetButtonsToNormal();
        }
    }

    public void ShowCursor(bool show)
    {
        if (!_isUsingController)
            Cursor.visible = show;
    }
}
