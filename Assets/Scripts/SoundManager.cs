﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager INSTANCE;

    public AudioSource bgmSource;
    public AudioSource fxSource;

    public AudioClip bgA;
    public AudioClip bgB;

    private void Awake(){

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFx(AudioClip clip) {
        fxSource.PlayOneShot(clip);
    }
}
