using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioPlayer;

    public AudioClip defaultMusic;

    private bool isFading;

    private float fadeProgress;
    
    private float targetVolume;
    // Start is called before the first frame update
    void Start()
    {
        targetVolume = audioPlayer.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleFade(float duration, bool isFadeIn)
    {
        if (isFading) return;
        fadeProgress = 0f;
        isFading = true;
        StartCoroutine(Fade(duration, isFadeIn));
    }

    public IEnumerator SetMusic(AudioClip track, float fadeDuration)
    {
        if (audioPlayer.isPlaying)
        {
            HandleFade(fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
            audioPlayer.clip = track;
            Debug.Log("starting Fade In");
            HandleFade(fadeDuration,true);
            audioPlayer.Play();
        }
    }

    private IEnumerator Fade(float duration, bool isFadeIn)
    {
        float startVolume = audioPlayer.volume;
        float endVolume = isFadeIn ? targetVolume : 0.0f;

        while (fadeProgress < 1.0f)
        {
            fadeProgress += Time.deltaTime / duration;
            audioPlayer.volume = Mathf.Lerp(startVolume, endVolume, fadeProgress);
            yield return null;
        }
        audioPlayer.volume = endVolume;
        isFading = false;
    }
}
