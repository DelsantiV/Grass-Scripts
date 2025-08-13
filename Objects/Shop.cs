using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Openable door;
    private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) player = other.GetComponent<Player>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            door.Unlock(openOnUnlock: false);
        }
    }
    private void Update()
    {
        if (player != null)
        {
            if (player.currentObject != null)
            {
                if (player.currentObject.TryGetComponent<ShopObject>(out _))
                {
                    door.Lock();
                    return;
                }
            }
            door.Unlock(openOnUnlock: false);
            Debug.Log(door.isLocked);
        }
    }
}
