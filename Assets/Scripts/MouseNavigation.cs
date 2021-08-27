using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNavigation : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _wandMeshRenderer;
    [SerializeField]
    GameObject _mainCamera;
    [SerializeField]
    float _distanceFar = 30f;
    [SerializeField]
    float _distanceClose = 15f;

    bool _wandIsUp = false;
    bool _glowIsFinished = true;

    List<GameObject> _diamondList;

    GameStateManager _gameStateManager;

    Color _originalColor;

    private void Start()
    {
        _gameStateManager = FindObjectOfType<GameStateManager>();

        _originalColor = _wandMeshRenderer.material.GetColor("_EmissionColor");
    }

    public void Navigate(bool isNavigating)
    {
        if (isNavigating)
        {
            _wandMeshRenderer.enabled = true;
            _wandIsUp = true;

            // Check if player is looking at the direction of some of the diamonds on the list and make the wand emissive

            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _diamondList = _gameStateManager.GetDiamondList();

                foreach (GameObject g in _diamondList)
                {
                    if (GameObject.ReferenceEquals(hitInfo.collider.gameObject, g))
                    {
                        float distance = Vector3.Distance(g.transform.position, transform.position);

                        if (_glowIsFinished) // Set the vibration patter according to distance from the diamond, if previous vibration is over
                        {
                            //Set the emission according to the distance from the diamond

                            if (distance >= _distanceFar)
                                StartCoroutine(GlowOnNavigation(10f, 1f, 1f));
                            else if (distance > _distanceClose && distance < _distanceFar)
                                StartCoroutine(GlowOnNavigation(25f, 0.6f, 0.6f));
                            else
                                StartCoroutine(GlowOnNavigation(40f, 0.3f, 0.3f));
                        }

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

    private IEnumerator GlowOnNavigation(float strength, float glowTime, float waitAfter)
    {
        _glowIsFinished = false;

        _wandMeshRenderer.material.SetColor("_EmissionColor", _originalColor * strength);
        yield return new WaitForSeconds(glowTime);

        _wandMeshRenderer.material.SetColor("_EmissionColor", _originalColor);
        yield return new WaitForSeconds(waitAfter);

        _glowIsFinished = true;
    }

    public void PutWandDown()
    {
        _wandMeshRenderer.enabled = false;
        _wandIsUp = false;
    }
}
