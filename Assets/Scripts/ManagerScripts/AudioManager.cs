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
    public AudioClip secondBgm;
    
    private bool isFading;

    private float fadeProgress;
    
    private float targetVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        if (bgm != null) { musicSource.clip = bgm; }
        musicSource.Play();
        musicSource.loop = true;
        targetVolume = musicSource.volume;
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

    public void PlayMusic(AudioClip clip, float fadeDuration)
    {
        StartCoroutine(SetMusic(clip, fadeDuration, targetVolume));
        //musicSource.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying) { musicSource.Stop(); }
    }
    
    
    private void HandleFade(float duration, bool isFadeIn)
    {
        if (isFading) return;
        fadeProgress = 0f;
        isFading = true;
        StartCoroutine(Fade(duration, isFadeIn));
    }

    public IEnumerator SetMusic(AudioClip track, float fadeDuration, float volume)
    {
            HandleFade(fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
            musicSource.clip = track;
            HandleFade(fadeDuration,true);
            musicSource.volume = volume;
            musicSource.Play();
    }

    private IEnumerator Fade(float duration, bool isFadeIn)
    {
        float startVolume = musicSource.volume;
        float endVolume = isFadeIn ? targetVolume : 0.0f;

        while (fadeProgress < 1.0f)
        {
            fadeProgress += Time.deltaTime / duration;
            musicSource.volume = Mathf.Lerp(startVolume, endVolume, fadeProgress);
            yield return null;
        }
        musicSource.volume = endVolume;
        isFading = false;
    }

    public void SetSecondBGM()
    {
        // bgm = secondBgm;
        musicSource.clip = secondBgm;
        musicSource.Play();
    }
    
}
