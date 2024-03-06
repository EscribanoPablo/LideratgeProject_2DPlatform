using System.Collections;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    
    [SerializeField] private AudioFile[] _musicFiles;
    [SerializeField] private AudioFile[] _sfxFiles;

    private AudioSource _audioSource;
    private AudioSource _musicSource;

    private float _soundVolume = 1;
    private float _musicVolume = 1;

    private void Awake()
    {
        if (_instance == null) _instance = this;

        //Initialize Audio Sources
        _audioSource = gameObject.AddComponent<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        
        _audioSource.playOnAwake = false;
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
    }


    /// <summary>
    /// Plays a music file using the given source. 
    /// If none is specified it uses the music source in sound manager.
    /// If a song is already playing, it makes a smooth transition.
    /// </summary>
    public static void PlayMusic(string name, AudioSource source = null)
    {
        _instance._PlayMusic(name, source);
    }

    /// <summary>
    /// Plays an audio file using the given source. 
    /// If none is specified it uses the sfx source in sound manager.
    /// </summary>
    public static void PlaySFX(string name, AudioSource source = null)
    {
        _instance._PlaySFX(name, source);
    }

    private void _PlayMusic(string name, AudioSource source = null)
    {
        if (source == null) source = _musicSource;

        AudioFile file = GetFileByName(name, _musicFiles);
        if (file != null)
        {
            StartCoroutine(TransitionToNewAudio(file, source));
        }
    }

    IEnumerator TransitionToNewAudio(AudioFile file, AudioSource source)
    {
        yield return SourceVolumeFade(source, 0, 1);

        source.clip = file.Clip;
        source.pitch = file.Pitch;
        source.Play();

        yield return SourceVolumeFade(source, file.Volume, 1);
    }

    
    private void _PlaySFX(string name, AudioSource source = null)
    {
        if (source == null) source = _audioSource;

        AudioFile file = GetFileByName(name, _sfxFiles);
        if (file == null)
            return;

        AudioClip clip = file.Clip;
        if (clip != null)
        {
            source.volume = file.Volume * _soundVolume;
            source.pitch = file.Pitch;
            source.clip = clip;
            source.Play();
        }
    }

    /// <summary>
    /// Fades the playing audioclip voulme on the given AudioSource
    /// to the desired volume in the given time.
    /// </summary>
    public IEnumerator SourceVolumeFade(AudioSource source, float vol, float time)
    {

        float initialVolume = source.volume;
        float currentVolume = source.volume;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            currentVolume = Mathf.Lerp(initialVolume, vol, elapsedTime / time);
            source.volume = currentVolume;

            yield return null;
        }
    }


    private AudioFile GetFileByName(string name, AudioFile[] files)
    {
        var file = files.First(x => x.Name == name);
        if (file != null)
            return file;
        
        Debug.LogError($"There is no audio file associated to {name}");
        return null;
    }


    //public void MusicVolumeChange(float amount)
    //{
    //    _musicVolume = amount;
    //}

    //public static void StopMusic()
    //{
    //    _instance._musicSource.Stop();
    //}

    //public void SFXVolumeChange(float amount)
    //{
    //    _soundVolume = amount;
    //}

}


[System.Serializable]
public class AudioFile
{
    public string Name;

    [SerializeField] private AudioClip[] Clips;
    [Range(0, 1)] public float Volume = 1;
    
    [Range(-3, 3)] [SerializeField] private float MinPitch = 1;
    [Range(-3, 3)] [SerializeField] private float MaxPitch = 1;

    public float Pitch => Random.Range(MinPitch, MaxPitch);
    public AudioClip Clip => Clips[Random.Range(0, Clips.Count())];
}


public static class AudioNames
{
    public static readonly string STEP = "STEP";
    public static readonly string LVLMUSIC = "LVLMUSIC";
    public static readonly string DIE = "DIE";
    public static readonly string BLOQUE = "BLOQUE";
    public static readonly string SHOOT = "SHOOT";
}

