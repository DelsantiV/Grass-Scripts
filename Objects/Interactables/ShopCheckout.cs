using System.Collections.Generic;
using UnityEngine;

public class ShopCheckout : MonoBehaviour
{
    public List<ShopObject> boughtObjects;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShopObject>(out ShopObject iobj))
        {
            boughtObjects.Add(iobj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StoreObject") && other.TryGetComponent<ShopObject>(out ShopObject iobj))
        {
            if (boughtObjects.Contains(iobj)) boughtObjects.Remove(iobj);
        }
    }

    public void CheckOut(out int prize)
    {
        prize = 0;
    }
}
