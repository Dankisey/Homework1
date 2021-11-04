using System.Collections;
using System.Collections.Generic;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        _robberIsInside = !_robberIsInside;

        if (collision.TryGetComponent<Robber>(out Robber robber))
        {
            if (_robberIsInside)
                StartAlarm();
            else
                EndAlarm();
        }
    }

    private void StartAlarm()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _volumeStep *= -1;
        }

        _currentCoroutine = StartCoroutine(ChangeVolume());
    }

    private void EndAlarm()
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
