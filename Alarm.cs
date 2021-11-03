using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeStep;
    [SerializeField] private float _secondsStep;

    private bool _robberInside = false;
    private AudioSource _audioSource;
    private Coroutine _currentCoroutine = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _robberInside = !_robberInside;   

        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            if (_robberInside)           
                StartAlarm();           
            else        
                EndAlarm();         
        }
    }

    private void StartAlarm()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(UpVolume());
    }

    private void EndAlarm()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(DownVolume());       
    }

    private IEnumerator UpVolume()
    {
        var waitForSecondsStep = new WaitForSeconds(_secondsStep);

        _audioSource.Play();

        while (_audioSource.volume < 1)
        {
            _audioSource.volume += _volumeStep;

            yield return waitForSecondsStep;
        }
    }

    private IEnumerator DownVolume()
    {
        var waitForSecondsStep = new WaitForSeconds(_secondsStep);

        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= _volumeStep;

            yield return waitForSecondsStep;
        }

        _audioSource.Pause();
    }
}
