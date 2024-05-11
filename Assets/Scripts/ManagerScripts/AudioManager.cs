using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------- Audio Sources --------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("-------- Audio Clips --------")]
    public AudioClip bgm;
    public AudioClip chaseBGM;
    public AudioClip respawn;
    public AudioClip checkpoint;
    public AudioClip jump;
    public AudioClip glide;
    public AudioClip windWoosh;
    public AudioClip levelComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null) { musicSource.clip = bgm; }
        musicSource.Play();
        musicSource.loop = true;
    }

    void update()
    {
        // If music is no longer playing, play the bgm again.
        if (!musicSource.isPlaying) { musicSource.clip = bgm; }
        musicSource.Play();
        musicSource.loop = true;
    }

    public void PlaySFX(AudioClip clip)
    {
        // Can hard code the volume mixing here
        if (clip == glide){ SFXSource.PlayOneShot(clip, 2.0f);}
        
        if (clip == jump){ SFXSource.PlayOneShot(clip, 0.5f);}
        
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying) { musicSource.Stop(); }
    }
    
}