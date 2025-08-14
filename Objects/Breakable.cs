using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] InteractableObject _breaker;
    private bool broke = false;

    private void Awake()
    {
        if ( _particleSystem == null ) TryGetComponent<ParticleSystem>(out _particleSystem);
        
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
