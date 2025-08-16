using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class MusicTrigger : MonoBehaviour
{
    [SerializeField] string musicTitle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.SwitchToQuest(musicTitle);
        }
    }
}
