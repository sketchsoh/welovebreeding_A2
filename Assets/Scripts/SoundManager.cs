using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip[] musicSounds;
    //[SerializeField] private AudioMixer audioMixer;
    private string pausedMusic;

    private AudioSource aS1, aS2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        InitializeSoundManager();
    }

    private void InitializeSoundManager()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        aS1 = sources[0];
        aS2 = sources[1];
        PlayMusicClip(MusicType.MainMenu);
    }

    public void PlayRandomSFXClip(AudioClip[] audioClips, Transform spawnTransform, bool persistent = false, float volume = 0.5f)
    {
        AudioClip audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        if (persistent) DontDestroyOnLoad(audioSource);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void TransitionMusicClip(MusicType musicType, float transitionTime = 1.25f, float targetVolume = 0.4f)
    {
        AudioSource nowPlaying = aS1;
        AudioSource target = aS2;
        if (!nowPlaying.isPlaying)
        {
            nowPlaying = aS2;
            target = aS1;
        }
        StartCoroutine(TransitionMusic(transitionTime, nowPlaying, target, musicType, targetVolume));
    }

    public void PlayMusicClip(MusicType musicType)
    {
        AudioSource nowPlaying = aS1;
        AudioClip audioClip = musicSounds[(int)musicType];
        nowPlaying.clip = audioClip;
        nowPlaying.Play();
    }

    private IEnumerator TransitionMusic(float duration, AudioSource nowPlaying, AudioSource target, MusicType musicType, float targetVolume = 0.4f)
    {
        float percent = 0f;
        float startingVolume = nowPlaying.volume;
        while (nowPlaying.volume > 0f)
        {
            nowPlaying.volume = Mathf.Lerp(startingVolume, 0, percent);
            percent += Time.deltaTime / duration;
            yield return null;
        }
        
        nowPlaying.Pause();
        if (!target.isPlaying)
        {
            AudioClip targetClip = musicSounds[(int)musicType];
            target.clip = targetClip;
            target.Play();
        }
        target.UnPause();
        percent = 0;

        while (target.volume < targetVolume)
        {
            target.volume = Mathf.Lerp(0, targetVolume, percent);
            percent += Time.deltaTime / duration;
            yield return null;
        }
    }
}

public enum MusicType
{
    MainMenu,
    Game,
    EOD
}

