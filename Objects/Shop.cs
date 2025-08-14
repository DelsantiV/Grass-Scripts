using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Openable door;
    private Player player;
    private bool shouldLockDoor = true;
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
        else if (other.gameObject.TryGetComponent<ShopObject>(out _))
        {
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        if (player != null && shouldLockDoor)
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
    public void UnlockDoor()
    {
        shouldLockDoor = false;
        door.Unlock(openOnUnlock: false);
    }
}
