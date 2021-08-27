using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    AudioSource _walkAudio;
    [SerializeField]
    AudioSource _runAudio;
    [SerializeField]
    AudioSource _jumpAudio;
    [SerializeField]
    AudioSource _endingAudio;
    [SerializeField]
    AudioSource _cricketsAudio;

    public void HandlePlayerMovementSounds(bool isMoving)
    {
        if (isMoving)
        {
            if (Input.GetButton("Run"))
            {
                _walkAudio.Stop();

                if (!_runAudio.isPlaying)
                    _runAudio.Play();
            }
            else if (!_walkAudio.isPlaying)
            {
                _runAudio.Stop();
                _walkAudio.Play();              
            }
        }
        else
        {
            _walkAudio.Stop();
            _runAudio.Stop();
        }
    }

    public void HandlePlayerJumpSound()
    {
        _jumpAudio.Play();
    }

    public void PlayEndingAudio()
    {
        _cricketsAudio.Stop();
        _endingAudio.Play();
    }
}
