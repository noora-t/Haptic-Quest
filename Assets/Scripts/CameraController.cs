using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _mouseSensitivity = 20f;
    [SerializeField]
    Transform _playerBody;

    float _mouseX, _mouseY;
    float _verticalAngle, _horizontalAngle = 0f;

    bool _isPaused = false;

    Vector3 _localAngles;
    Vector3 _currentPlayerAngles;

    GameControls _gameControls;

    private void Start()
    {
        _gameControls = FindObjectOfType<GameControls>();
    }

    void Update()
    {
        // Check if the game is paused
        if (!_isPaused)
        {         
            // Camera look up/down

            if (_gameControls.IsUsingController)
                _mouseY = -Input.GetAxis("5th") * _mouseSensitivity;
            else
                _mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _localAngles = transform.localEulerAngles;
            _verticalAngle = Mathf.Clamp(_mouseY + _verticalAngle, -45.0f, 45.0f);
            _localAngles.x = _verticalAngle;
            transform.localEulerAngles = _localAngles;

            // Turn camera by turning player

            if (_gameControls.IsUsingController)
                _mouseX = Input.GetAxis("4th") * _mouseSensitivity;
            else
                _mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;

            _horizontalAngle += _mouseX;

            if (_horizontalAngle > 360) _horizontalAngle -= 360.0f;
            if (_horizontalAngle < 0) _horizontalAngle += 360.0f;

            _currentPlayerAngles = _playerBody.transform.localEulerAngles;
            _currentPlayerAngles.y = _horizontalAngle;
            _playerBody.transform.localEulerAngles = _currentPlayerAngles;   
        }
    }

    // Handle camera movement when diamond is found
    public void DiamondFound(Collider diamond)
    {
        _isPaused = true;
        
        Quaternion toRotation = Quaternion.LookRotation(diamond.transform.position - transform.position);

        // Rotate camera towards found diamond
        StartCoroutine(LerpCamera(toRotation)); 
    }

    IEnumerator LerpCamera(Quaternion toRotation)
    {
        float elapsedTime = 0;
        float waitTime = 1f;
        Quaternion currentRot = transform.rotation;

        while (elapsedTime < waitTime)
        {
            transform.rotation = Quaternion.Lerp(currentRot, toRotation, (elapsedTime / waitTime));
            Camera.main.fieldOfView -= elapsedTime * 0.1f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = toRotation;

        yield return null;
    }

    // Freeze the camera movement
    public void FreezeCamera(bool isPaused)
    {
        this._isPaused = isPaused;
    }
}
