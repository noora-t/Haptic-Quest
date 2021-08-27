using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    AudioController _audioController;
    UIController _uiController;
    GameControls _gameControls;

    int _goalScore = 3;
    int _score;
    
    Text _text;

    void Start()
    {
        _audioController = FindObjectOfType<AudioController>();
        _uiController = FindObjectOfType<UIController>();
        _gameControls = FindObjectOfType<GameControls>();

        _text = GetComponent<Text>();
        _score = 0;
    }

    void Update()
    {
        _text.text = "Diamonds found: " + _score + "/3";
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetGoalScore()
    {
        return _goalScore;
    }

    // Update the score and check if all diamonds have been found
    public void AddToScore()
    {
        _score++;

        // Check if game is finished
        if (_score == _goalScore)
            StartCoroutine(WaitUntilEndingGame());
    }

    IEnumerator WaitUntilEndingGame()
    {
        yield return new WaitForSeconds(2f);

        _audioController.PlayEndingAudio();
        _gameControls.ShowCursor(true);
        _uiController.HandleMenus(true, "endingObjects");
    }
}
