using UnityEngine;
using System.Collections;

public class BirdAudio : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    
    private AudioSource audioSource;
    
    void OnEnable()
    {
        StartCoroutine(InitAudio());
    }

    IEnumerator InitAudio()
    {
        yield return new WaitForSeconds(0.1f);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.loop = true;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        //force hardcoded values:(optionally, can be set in inspector)
        //audioSource.minDistance = 2f;
        //audioSource.maxDistance = 40f;
        
        PlaySong();
    }
    
    void PlaySong()
    {
        if (audioSource == null) return;
        audioSource.clip = Random.value < .5f ? song1 : song2;
        audioSource.Play();
    }
}