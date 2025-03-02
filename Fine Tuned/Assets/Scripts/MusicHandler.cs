using UnityEngine;
using System.Collections; // Add this for IEnumerator

public class MusicHandler : MonoBehaviour
{
    public AudioClip musicClip; // Assign your music clip in the inspector
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure this object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        StartCoroutine(PlayMusicWithDelay()); // Start the coroutine
    }

    public void UpdateSong(AudioClip newSong)
    {
        audioSource.Stop();
        audioSource.clip = newSong;
        audioSource.Play();
    }

    private IEnumerator PlayMusicWithDelay() // Coroutine to handle delay
    {
        yield return new WaitForSeconds(30); // Wait for 5 seconds
        audioSource.Play(); // Play the music after the delay
    }
}
