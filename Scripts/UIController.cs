using UnityEngine;

public class UIController : MonoBehaviour
{
    GameObject[] _pauseObjects;
    GameObject[] _endingObjects;

    void Start()
    {
        _pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        _endingObjects = GameObject.FindGameObjectsWithTag("ShowOnEnding");       
    }

    // Show and hide menus
    public void HandleMenus(bool activate, string list)
    {      
        if (list.Equals("pauseObjects"))
        {
            foreach (GameObject g in _pauseObjects)
                g.SetActive(activate);
            
        }
        else if (list.Equals("endingObjects"))
        {
            foreach (GameObject g in _endingObjects)
                g.SetActive(activate);
        }       
    }
}
