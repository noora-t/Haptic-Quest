using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerNavigation : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _wandMeshRenderer;
    [SerializeField]
    float _distanceFar = 30f;
    [SerializeField]
    float _distanceClose = 15f;

    bool _wandIsUp = false;
    bool _vibrateIsFinished = true;

    List<GameObject> _diamondList;

    GameStateManager _gameStateManager;

    float _wandAngle = 10f;

    private void Start()
    {
        _gameStateManager = FindObjectOfType<GameStateManager>();
    }

    public void Navigate(bool isNavigating)
    {
        if (isNavigating)
        {
            _wandMeshRenderer.enabled = true;
            _wandIsUp = true;

            // Check if player is looking at the direction of some of the diamonds on the list and make the controller vibrate

            _diamondList = _gameStateManager.GetDiamondList();

            foreach (GameObject g in _diamondList)
            {
                if (Vector3.Angle(transform.forward, g.transform.position - transform.position) < _wandAngle)
                {
                    float distance = Vector3.Distance(g.transform.position, transform.position);

                    if (_vibrateIsFinished) // Set the vibration patter according to distance from the diamond, if previous vibration is over
                    {
                        if (distance >= _distanceFar)
                            StartCoroutine(VibrateOnNavigation(0.35f, 0.03f, 0.2f, 0.08f, 0.15f, 1.5f));
                        else if (distance > _distanceClose && distance < _distanceFar)
                            StartCoroutine(VibrateOnNavigation(0.43f, 0.06f, 0.2f, 0.08f, 0.15f, 1f));
                        else
                            StartCoroutine(VibrateOnNavigation(0.5f, 0.1f, 0.2f, 0.08f, 0.15f, 0.5f));
                    }
                }
            }
        }
        else
        {
            if (_wandIsUp) // Put the wand down
            {
                _wandMeshRenderer.enabled = false;
                _wandIsUp = false;
            }
        }
    }

    /** 
    Controller vibration motors:
        GamePad.SetVibration(PlayerID playerID, float leftMotor, float rightMotor);
        Value = amblitude (value between 0 and 1)
        Left motor = low frequency
        Right motor = high frequency
    **/
    IEnumerator VibrateOnNavigation(float amblitudeLeft, float amblitudeRight, float vibrateTimeLeft,
        float vibrateTimeRight, float waitBetween, float waitAfter)
    {
        _vibrateIsFinished = false;

        GamePad.SetVibration(0, amblitudeLeft, 0);
        yield return new WaitForSeconds(vibrateTimeLeft);

        GamePad.SetVibration(0, 0, 0);
        yield return new WaitForSeconds(waitBetween);

        GamePad.SetVibration(0, 0, amblitudeRight);
        yield return new WaitForSeconds(vibrateTimeRight);

        GamePad.SetVibration(0, 0, 0);
        yield return new WaitForSeconds(waitAfter);

        _vibrateIsFinished = true;
    }

    internal void PutWandDown()
    {
        _wandMeshRenderer.enabled = false;
        _wandIsUp = false;
    }
}
