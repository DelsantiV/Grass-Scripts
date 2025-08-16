using System.Collections;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] MusicSO musicSO;
    private AudioClip baseMusic;
    private AudioSource audioSource;
    private int currentMusicIndex;
    void Awake()
    {
        if (Instance == null) {Instance = this;}
        else Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
        baseMusic = musicSO.baseMusics[0];
        currentMusicIndex = 0;
    }
    public void StartGameMusic()
    {
        audioSource.clip = baseMusic;
        audioSource.volume = 0;
        StartCoroutine(MusicFadeIn());
    }
    private IEnumerator MusicFadeIn()
    {
        audioSource.Play();
        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator MusicFadeOut(bool fadeInAfter, AudioClip newClip = null)
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= 0.001f;
            yield return new WaitForSeconds(0.01f);
        }
        audioSource.volume = 0;
        if (newClip != null) audioSource.clip = newClip;
        if (fadeInAfter) StartCoroutine(MusicFadeIn());
    }
    private IEnumerator BackToMainMusic(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(MusicFadeOut(true, baseMusic));
    }
    public void SwitchToQuest(string music, float time = 120)
    {
        QuestMusic questMusic = musicSO.questMusics.ToList().Find(m => m.title == music);
        if (questMusic != null)
        {
            audioSource.clip = questMusic.audioClip;
            audioSource.Play();
            StartCoroutine(BackToMainMusic(time));
        }
    }
    public void GoToNextMusic()
    {
        currentMusicIndex++;
        currentMusicIndex %= musicSO.baseMusics.Length;
        AudioClip nextMusic = musicSO.baseMusics[currentMusicIndex];
        StartCoroutine(MusicFadeOut(true, nextMusic));
    }
}
