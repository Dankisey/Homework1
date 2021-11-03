using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private float _volumeStep;

    private bool _robberInside = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        _audioSource.Play();
    
        StartCoroutine(UpVolume());
    }

    private void EndAlarm()
    {
        StartCoroutine(DownVolume());

        _audioSource.Pause();
    }

    private IEnumerator UpVolume()
    {
        while(_audioSource.volume < 1)
        {
            _audioSource.volume += _volumeStep;

            yield return null;
        }
    }

    private IEnumerator DownVolume()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= _volumeStep;

            yield return null;
        }
    }
}
