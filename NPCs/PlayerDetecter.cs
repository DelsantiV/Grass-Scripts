using UnityEngine;
using UnityEngine.Events;

public class PlayerDetecter : MonoBehaviour
{
    private SphereCollider detecter;
    public UnityEvent OnPlayerDetected;
    public Player player;
    public bool isPlayerInRange { get => player != null; }

    private void Awake()
    {
        detecter = GetComponent<SphereCollider>();
        detecter.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player>();
            OnPlayerDetected.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }
    public void EnableDetecter(bool enable)
    {
        detecter.enabled = enable;
    }
}
