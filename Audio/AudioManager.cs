using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioClip baseMusic;
    [SerializeField] AudioClip questMusic;
    private AudioSource audioSource;
    void Awake()
    {
        if (Instance == null) {Instance = this;}
        else Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    public void StartGameMusic()
    {
        audioSource.clip = baseMusic;
        audioSource.volume = 0;
        audioSource.Play();
        StartCoroutine(MusicFadeIn());
    }
    private IEnumerator MusicFadeIn()
    {
        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += 0.001f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void SwitchToQuest()
    {
        audioSource.clip = questMusic;
        audioSource.Play();
    }
}
