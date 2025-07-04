using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Background Music")]
    public AudioClip bgMusicClip;
    [Range(0f, 1f)]
    public float bgMusicVolume = 0.5f;

    [Header("SFX")]
    public AudioClip clickClip;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Header("UI Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.clip = bgMusicClip;
        _musicSource.volume = bgMusicVolume;
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.volume = sfxVolume;
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;

        RegisterButton(button1);
        RegisterButton(button2);
        RegisterButton(button3);
        RegisterButton(button4);
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (bgMusicClip != null && !_musicSource.isPlaying)
            _musicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (_musicSource.isPlaying)
            _musicSource.Stop();
    }

    public void PlayClick()
    {
        if (clickClip != null)
            _sfxSource.PlayOneShot(clickClip);
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp01(volume);
    }

    private void RegisterButton(Button btn)
    {
        if (btn != null)
            btn.onClick.AddListener(PlayClick);
    }
}
