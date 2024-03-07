using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnStart : MonoBehaviour
{
    public string musicToPlay;
    private AudioSource musicSource;


    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        SoundManager.PlayMusic(musicToPlay, musicSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
