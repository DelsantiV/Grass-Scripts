using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Openable door;
    [SerializeField] Openable checkoutBarrier;
    private Player player;
    private bool isFirstTime = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkoutBarrier.Close();
            player = other.GetComponent<Player>();
            if (isFirstTime)
            {
                isFirstTime = false;
                player.SetWorldMessage("In the shop, you can buy everything you need ! A fresh orange juice, nice vegetables or a braand new deadly chainsaw ! Put everything on the treadmill and talk to Sandrine to buy objects ! DO NOT TRY TO STEAL !");
            }
        }
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
        }
    }
}
