using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource _audioSource;
    public static PlaySound Instance;
    public AudioClip terminatedSound;
    public List<AudioClip> playCardSounds;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundTerminated()
    {
        _audioSource.PlayOneShot(terminatedSound);
    }

    public void PlaySoundPlaceCard()
    {
        _audioSource.PlayOneShot(playCardSounds[Random.Range(0, playCardSounds.Count)]);
    }
}
