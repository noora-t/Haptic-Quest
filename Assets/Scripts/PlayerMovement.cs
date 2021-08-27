using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject _mainCamera;

    [SerializeField]
    float _walkSpeed = 5f;
    [SerializeField]
    float _runSpeed = 18f;
    [SerializeField]
    float _jumpSpeed = 1f;

    float _gravity = 5f;

    bool _isPaused = false;

    AudioController _audioController;
    ScoreManager _scoreManager;
    MouseNavigation _mouseNavigation;
    ControllerNavigation _controllerNavigation;
    GameControls _gameControls;
    GameStateManager _gameStateManager;

    CharacterController _controller;
    Vector3 _moveDirection;

    Quaternion _originalRotation;

    private void Start()
    {
        _audioController = FindObjectOfType<AudioController>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _controller = gameObject.GetComponent<CharacterController>();
        _gameControls = FindObjectOfType<GameControls>();
        _gameStateManager = FindObjectOfType<GameStateManager>();

        _mouseNavigation = GetComponentInParent<MouseNavigation>();
        _controllerNavigation = GetComponentInParent<ControllerNavigation>();
    }

    void Update()
    {
        if (!_isPaused) // Game is not paused so player can move and navigate
        {
            if (_controller.isGrounded) // Player is grounded and can move and jump
                HandleMoveDirectionAndSpeed();

            // Apply gravity and move the charactercontroller
            _moveDirection.y -= _gravity * Time.deltaTime;
            _moveDirection = transform.TransformDirection(_moveDirection);
            _controller.Move(_moveDirection);

            // If player is moving, play footstep audio
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("HorizontalMouse") != 0 
                || Input.GetAxis("VerticalMouse") != 0) 
                _audioController.HandlePlayerMovementSounds(true);
            else
                _audioController.HandlePlayerMovementSounds(false);

            // Check if player is pressing the R1 button to navigate
            if (Input.GetButton("Navigate"))
            {
                if (_gameControls.IsUsingController)
                    _controllerNavigation.Navigate(true);
                else
                    _mouseNavigation.Navigate(true);
            }
                
            else
            {
                if (_gameControls.IsUsingController)
                    _controllerNavigation.Navigate(false);
                else
                    _mouseNavigation.Navigate(false);
            }
                
        }
    }

    //Run: L1, Jump: X
    private void HandleMoveDirectionAndSpeed()
    {
        // Move and look around with joysticks
        if (_gameControls.IsUsingController)
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        else
            _moveDirection = new Vector3(Input.GetAxis("HorizontalMouse"), 0, Input.GetAxis("VerticalMouse"));

        if (_moveDirection.sqrMagnitude > 1.0f)
            _moveDirection.Normalize();

        // Check if player wants to walk or run, and set the speed
        if (Input.GetButton("Run"))
            _moveDirection = _moveDirection * _runSpeed * Time.deltaTime;
        else
            _moveDirection = _moveDirection * _walkSpeed * Time.deltaTime;

        // Check if player wants to jump
        if (Input.GetButtonDown("Jump"))
        {
            _moveDirection.y = _jumpSpeed;
            _audioController.HandlePlayerJumpSound();
        }
    }

    // Player entered a diamond's trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Diamond"))
        {
            if(_gameControls.IsUsingController)
            {
                // Set other vibrations off and play found vibration
                //_vibrateIsFinished = false;
                GamePad.SetVibration(0, 0, 0);
                //StartCoroutine(VibrateOnDiamondFound());
            }

            // Put wand down
            if (_gameControls.IsUsingController)
                _controllerNavigation.PutWandDown();
            else
                _mouseNavigation.PutWandDown();
            

            // Handle diamond list and behavior
            _gameStateManager.RemoveFromDiamondList(other.gameObject);
            other.GetComponent<Diamond>().DiamondFound();

            // Pause player movement
            _audioController.HandlePlayerMovementSounds(false);
            _isPaused = true;

            // Handle camera rotation
            _originalRotation = _mainCamera.transform.rotation;
            _mainCamera.GetComponent<CameraController>().DiamondFound(other);

            // Activate animations on diamond and score text, and re-enable free player and camera movement
            StartCoroutine(HandleDiamondFound(other));
        }
    }

    IEnumerator HandleDiamondFound(Collider other)
    {
        // Activate diamond animation
        other.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(4.5f);

        // Add to score and activate score text animation
        _scoreManager.AddToScore();
        GameObject.Find("ScoreText").GetComponent<Animator>().SetTrigger("Scale");
        yield return new WaitForSeconds(2f);

        // Disable found diamond
        other.gameObject.SetActive(false);

        // If goal score is not reached, re-enable player and camera movement
        if (!_scoreManager.GetScore().Equals(_scoreManager.GetGoalScore()))
        {
            _isPaused = false;
            _mainCamera.GetComponent<CameraController>().FreezeCamera(false);
            _mainCamera.transform.rotation = _originalRotation;
            Camera.main.fieldOfView = 60f;
        }

        //_vibrateIsFinished = true;

        yield return null;
    }

    //IEnumerator VibrateOnDiamondFound()
    //{
    //    GamePad.SetVibration(0, 0.1f, 0.03f);
    //    yield return new WaitForSeconds(0.5f);
    //    GamePad.SetVibration(0, 0, 0);
    //}

    public void SetFreeze(bool isFrozen)
    {
        this._isPaused = isFrozen;
        _audioController.HandlePlayerMovementSounds(false);
    }
}
