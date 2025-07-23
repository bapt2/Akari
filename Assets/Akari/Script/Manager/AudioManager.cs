using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public bool isPlaying;
    public float transitionTime;
    public float defaultVolume;

    public AudioClip dayMusic;
    public AudioClip nightMusic;
    public AudioSource audioSource;

    public AudioMixerGroup soundEffectMixer;
    public AudioMixerGroup dayMusicMixer;
    public AudioMixerGroup nightMusicMixer;
    public DayNightCycle dayNightCycle;




    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"Plus d'une instance d' {this} dans la scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SmoothMusicTransition(dayMusic, dayMusicMixer));
    }

    public IEnumerator SmoothMusicTransition(AudioClip clip, AudioMixerGroup audioMixerGroup)
    {

        float percentage = 0;
        
        if (audioSource.clip != null)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume = Mathf.Lerp(defaultVolume, 0, percentage);
                percentage += Time.deltaTime * transitionTime;
                yield return null;
            }

            audioSource.Pause();
        }


        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();

        percentage = 0;

        while (audioSource.volume < defaultVolume)
        {
            audioSource.volume = Mathf.Lerp(0, defaultVolume, percentage);
            percentage += Time.deltaTime * transitionTime;
            yield return null;
        }
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        AudioSource _audioSource = tempGO.AddComponent<AudioSource>();
        _audioSource.clip = clip;
        _audioSource.outputAudioMixerGroup = soundEffectMixer;
        _audioSource.Play();
        Destroy(_audioSource, clip.length);
        return _audioSource;
    }
}
