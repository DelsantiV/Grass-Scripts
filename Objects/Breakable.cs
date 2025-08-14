using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] InteractableObject _breaker;
    [SerializeField] AudioClip audioClip;
    private AudioSource audioSource;
    private bool broke = false;

    private void Awake()
    {
        if ( _particleSystem == null ) TryGetComponent<ParticleSystem>(out _particleSystem);
        if (audioClip != null) audioSource = gameObject.GetOrAddComponent<AudioSource>();
    }
    private void Start()
    {
        if (_breaker != null)
        {
            _breaker.OnInteracted.AddListener(Break);
        }
    }
    private void Break()
    {
        if (broke) return;
        if (_breaker != null)
        {
            _breaker.OnInteracted.RemoveListener(Break);
        }
        if (_particleSystem != null) { _particleSystem.Play(); }
        broke = true;
        if (audioClip != null) audioSource.PlayOneShot(audioClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (broke)
            {
                if (_particleSystem != null) _particleSystem.Play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (broke)
            {
                if (_particleSystem != null) _particleSystem.Stop();
            }
        }
    }
}
