using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeStep;
    [SerializeField] private float _secondsStep;

    private bool _robberIsInside = false;

    private AudioSource _audioSource;
    private Coroutine _currentCoroutine = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _robberIsInside = true;

        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            ChangeActivity();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _robberIsInside = false;

        if (collision.TryGetComponent<Robber>(out Robber robber))
        {          
            ChangeActivity();
        }
    }

    private void ChangeActivity()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _volumeStep *= -1;
        }

        _currentCoroutine = StartCoroutine(ChangeVolume());
    }

    private IEnumerator ChangeVolume()
    {
        var waitForSecondsStep = new WaitForSeconds(_secondsStep);

        if (_robberIsInside)
            _audioSource.Play();

        while (_audioSource.volume > 0 || _audioSource.volume < 1)
        {
            _audioSource.volume += _volumeStep;

            yield return waitForSecondsStep;
        }

        if (_robberIsInside == false)       
            _audioSource.Pause();        
    }
}
