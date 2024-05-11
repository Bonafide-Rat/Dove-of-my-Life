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
        if (bgm != null) {musicSource.clip = bgm;}
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == glide){ SFXSource.PlayOneShot(clip, 2.0f);}
        SFXSource.PlayOneShot(clip);
    }
}
