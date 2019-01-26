using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    public AudioSource bgmSource;
    public AudioSource fxSource;

    public AudioClip bgA;
    public AudioClip bgB;
    private AudioClip currentBgAudio;

    private void Awake(){
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentBgAudio = bgA;
        bgmSource.clip = currentBgAudio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFx(AudioClip clip) {
        fxSource.PlayOneShot(clip);
    }

    public void StopBg() {
        bgmSource.Stop();
    }

    public void ToggleBgAudio () {
        if (currentBgAudio == bgA) {
            currentBgAudio = bgB;
        }  else {
            currentBgAudio = bgA;
        }
        bgmSource.clip = currentBgAudio;
        bgmSource.Stop();
        bgmSource.Play();
    }
}
